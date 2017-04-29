/*!
 * ng-image-input-with-preview v0.0.6
 * https://github.com/deiwin/ngImageInputWithPreview
 *
 * A FileReader based directive to easily preview and upload image files.
 *
 * Copyright 2015, Deiwin Sarjas <deiwin.sarjas@gmail.com>
 * Released under the MIT license
 */
(function (angular, undefined) {
    'use strict';

    // src/js/fileReader.service.js
    (function () {
        'use strict';
        var module = angular.module('fileReaderService', []);

        // Copied from the following link with onProgress excluded because it's not needed
        // http://odetocode.com/blogs/scott/archive/2013/07/03/building-a-filereader-service-for-angularjs-the-service.aspx
        module.factory('fileReader', ['$q', '$rootScope',
          function ($q, $rootScope) {
              var onLoad = function (reader, deferred, scope) {
                  return function () {
                      scope.$apply(function () {
                          deferred.resolve(reader.result);
                      });
                  };
              };

              var onError = function (reader, deferred, scope) {
                  return function () {
                      scope.$apply(function () {
                          deferred.reject(reader.result);
                      });
                  };
              };

              var getReader = function (deferred, scope) {
                  var reader = new FileReader();
                  reader.onload = onLoad(reader, deferred, scope);
                  reader.onerror = onError(reader, deferred, scope);
                  return reader;
              };

              var readAsDataURL = function (file, scope) {
                  var deferred = $q.defer();
                  var reader = getReader(deferred, scope);
                  reader.readAsDataURL(file);

                  return deferred.promise;
              };

              return {
                  readAsDataUrl: readAsDataURL
              };
          }
        ]);
    })();

    // src/js/imageWithPreview.directive.js
    /*jshint -W072 */
    // ^ ignore jshint warning about link method having too many parameters
    (function () {
        'use strict';
        var module = angular.module('ngImageInputWithPreview', [
          'fileReaderService'
        ]);

        module.directive('uploadImage', ['fileReader', '$q', 
          function (fileReader, $q) {
              var DEFAULT_MIMETYPES = 'image/png,image/jpeg';
              var NOT_AN_IMAGE = 'this-is-not-an-image';
              var isAnAllowedImage = function (allowedTypes, file) {
                  if (!allowedTypes) {
                      allowedTypes = DEFAULT_MIMETYPES;
                  }
                  var allowedTypeArray = allowedTypes.split(',');
                  return allowedTypeArray.some(function (allowedType) {
                      if (allowedType === file.type) {
                          return true;
                      }
                      var allowedTypeSplit = allowedType.split('/');
                      var fileTypeSplit = file.type.split('/');
                      return allowedTypeSplit.length === 2 && fileTypeSplit.length === 2 && allowedTypeSplit[1] === '*' &&
                        allowedTypeSplit[0] === fileTypeSplit[0];
                  });
              };
              var createResolvedPromise = function () {
                  var d = $q.defer();
                  d.resolve();
                  return d.promise;
              };
              /*----------------------start ---------*/
              var URL = window.URL || window.webkitURL;

              var getResizeArea = function () {
                  var resizeAreaId = 'fileupload-resize-area';

                  var resizeArea = document.getElementById(resizeAreaId);

                  if (!resizeArea) {
                      resizeArea = document.createElement('canvas');
                      resizeArea.id = resizeAreaId;
                      resizeArea.style.visibility = 'hidden';
                      document.body.appendChild(resizeArea);
                  }

                  return resizeArea;
              }

              var resizeImage = function (origImage, options) {
                  var maxHeight = options.resizeMaxHeight || 300;
                  var maxWidth = options.resizeMaxWidth || 250;
                  var quality = options.resizeQuality || 0.7;
                  var type = options.resizeType || 'image/jpg';

                  var canvas = getResizeArea();

                  var height = origImage.height;
                  var width = origImage.width;

                  // calculate the width and height, constraining the proportions
                  if (width > height) {
                      if (width > maxWidth) {
                          height = Math.round(height *= maxWidth / width);
                          width = maxWidth;
                      }
                  } else {
                      if (height > maxHeight) {
                          width = Math.round(width *= maxHeight / height);
                          height = maxHeight;
                      }
                  }

                  canvas.width = width;
                  canvas.height = height;

                  //draw image on canvas
                  var ctx = canvas.getContext("2d");
                  ctx.drawImage(origImage, 0, 0, width, height);

                  // get the data from canvas as 70% jpg (or specified type).
                  return canvas.toDataURL(type, quality);
              };
              var id = "";
              var createImage = function (url, callback) {
                  var image = new Image();
                  image.onload = function () {
                      callback(image);
                  };
                  image.src = url;
              };

              var fileToDataURL = function (file) {
                  var deferred = $q.defer();
                  var reader = new FileReader();
                  reader.onload = function (e) {
                      deferred.resolve(e.target.result);
                  };
                  reader.readAsDataURL(file);
                  return deferred.promise;
              };
              /*--------------end-----------------*/
              return {
                  restrict: 'A',
                  require: 'ngModel',
                  scope: {
                      image: '=ngModel',
                      allowedTypes: '@accept',
                      dimensionRestrictions: '&dimensions',
                  },
                  link: function ($scope, element, attrs, ngModel) {
                      /*--------------start-----------------*/
                      var doResizing = function (imageResult, callback) {
                          createImage(imageResult.url, function (image) {
                              var dataURL = resizeImage(image, $scope);
                              $("#" + id + "bc").css("background-image", "url(" + dataURL + ")");
                              imageResult.resized = {
                                  dataURL: dataURL,
                                  type: dataURL.match(/:(.+\/.+);/)[1],
                              };
                              callback(imageResult);
                          });
                      };
                      /*--------------end-----------------*/
                      element.bind('change', function (event) {
                          id = element.attr("id");
                          var file = (event.srcElement || event.target).files[0];
                          // the following link recommends making a copy of the object, but as the value will only be changed
                          // from the view, we don't have to worry about making a copy
                          // https://docs.angularjs.org/api/ng/type/ngModel.NgModelController#$setViewValue                         
                          /*--------------start-----------------*/
                          if (file) {
                              ngModel.$setViewValue(file, 'change');
                           }
                          if (file && file.type.indexOf("image") >= 0 && file.size<= 1024 * 5120)
                          {
                              $("#bc").css("background-image", "url('/Content/images/default.svg')");
                              var imageResult = {
                                  file: file,
                                  url: URL.createObjectURL(file)
                              };
                              fileToDataURL(file).then(function (dataURL) {
                                  imageResult.dataURL = dataURL;
                              });
                              doResizing(imageResult, function (imageResult) {
                                  // applyScope(imageResult);
                                  if ($scope.$parent.product) {
                                      $scope.$parent.product.Image = imageResult.resized.dataURL;
                                  }
                                  if ($scope.$parent.customer) {
                                      $scope.$parent.customer.Logo = imageResult.resized.dataURL;
                                  }
                              });
                          }
                          /*--------------end-----------------*/
                      });

                      ngModel.$parsers.push(function (file) {
                          if (!file) {
                              return file;
                          }
                          if (!isAnAllowedImage($scope.allowedTypes, file)) {
                              return NOT_AN_IMAGE;
                          }
                          return {
                              fileReaderPromise: fileReader.readAsDataUrl(file, $scope)
                          };
                      });
                      $scope.$watch('image', function (value) {
                          if (value && typeof value === 'string') {
                              $scope.image = {
                                  src: value,
                                  isPath: true,
                              };
                          }
                      });
                      ngModel.$validators.image = function (modelValue, viewValue) {
                          var value = modelValue || viewValue;
                          return value !== NOT_AN_IMAGE;
                      };
                      ngModel.$validators.size = function (modelValue, viewValue) {
                          if (!viewValue) {
                              return true;
                          } else {
                              if (!viewValue.size) {
                                  return true;
                              }
                              return viewValue.size <= 5120 * 1024;
                          }
                      };
                      ngModel.$asyncValidators.parsing = function (modelValue, viewValue) {
                          var value = modelValue || viewValue;
                          if (!value || !value.fileReaderPromise) {
                              return createResolvedPromise();
                          }
                          // This should help keep the model value clean. At least I hope it does
                          value.fileReaderPromise.finally(function () {
                              delete value.fileReaderPromise;
                          });
                          return value.fileReaderPromise.then(function (dataUrl) {
                              value.src = dataUrl;
                          }, function () {
                              return $q.reject('Failed to parse');
                          });
                      };
                  }
              };
          }
        ]);
    })();
})(window.angular);

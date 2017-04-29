angular.module("app").run(
    ["$rootScope", "growl", "$location", "$window", "$q", "$timeout",
        function ($rootScope, growl, $location, $window, $q, $timeout) {
            $rootScope.checkIfEdit = function () {
                var activeBtn = $(".showItemDisable:enabled");
                if (activeBtn.length) {
                    growl.error('Please save data before you do other things');
                    setTimeout(function () {
                        $(".overview").addClass("active").focus().siblings().removeClass("active");
                    });
                    activeBtn.focus();
                    return false;
                } else {
                    return true;
                }
            }
            $rootScope.checkIfChangeTab = function ($event, customer) {
                //$event.preventDefault();
                //$event.stopPropagation();
                if (!customer.Id) {
                    growl.error("Please create a customer first!");
                    setTimeout(function () {
                        $(".overview").addClass("active").focus().siblings().removeClass("active");
                    });
                    return false;
                } else {
                    return true;
                }
            }
            $rootScope.backToProduct = true;
            setTimeout(function () {
                var urlArray = $location.absUrl().split("/");

                var url = urlArray[urlArray.length - 1];

                $rootScope.backToProduct = true;
                if (!$window.localStorage.getItem("state")) {
                    $rootScope.quote = false;
                    $rootScope.qtoorder = false;
                    $rootScope.order = false;
                    $rootScope.product = false;
                    return;
                }
                if ($window.localStorage.getItem("state") === "quote") {
                    if (url === "products") {
                        $("#nav li").eq(1).removeClass("nav-display");
                        $("#bs-example .navbar-form").removeClass("nav-display")

                        return;
                    } else {
                        $("#nav li").eq(4).removeClass("nav-display");
                        $("#nav li").eq(1).removeClass("nav-display");
                        return;
                    }
                }
                if ($window.localStorage.getItem("state") === "order") {
                    if (url === "products") {
                        $("#nav li").eq(1).removeClass("nav-display");
                        $("#bs-example .navbar-form").removeClass("nav-display")
                        return;
                    } else {
                        $("#nav li").eq(1).removeClass("nav-display");
                        $("#nav li").eq(3).removeClass("nav-display");

                        return;
                    }
                }
                if ($window.localStorage.getItem("state") === "qtoorder") {
                    if (url === "qtoorder") {
                        //$("#nav li").eq(1).removeClass("nav-display");
                        $("#nav li").eq(2).removeClass("nav-display");
                        //$("#bs-example .navbar-form").removeClass("nav-display")
                        return;
                    }
                    if (url === "order") {
                        $("#nav li").eq(2).removeClass("nav-display");
                        $("#nav li").eq(3).removeClass("nav-display");
                        //$("#bs-example .navbar-form").removeClass("nav-display")
                        return;
                    }
                }
                if ($window.localStorage.getItem("state") === "qandorder") {
                    if (url === "qandorder") {
                        $("#nav li").eq(5).removeClass("nav-display");
                        return;
                    }
                    if (url === "order") {
                        $("#nav li").eq(5).removeClass("nav-display");
                        $("#nav li").eq(6).removeClass("nav-display");
                        return;
                    }
                }
            }, 1000);
        }])
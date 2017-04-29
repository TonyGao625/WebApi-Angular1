 $(function(){
	 $(".productList").on('click','.card-box-wrap .card-img img',function(){
	
	  if($(window).width()<=991){
	 	 $("#myModal").css('top','10%')
     	 $("#myModal").modal({
			 keyboard:true
		 })
	   }
	    if($(window).width()>992 && $(window).width()<1169)
	    {
	 
            $(this).parent().find('.card-img-info').css('display','block')
	      	   
	    }
	 })
     $(".productList").on('mouseover','.card-box-wrap .card-img img',function(){

    	 if($(window).width()>=1170){
    		   $(this).parent().find('.card-img-info').show();

    	 }

	 
	 })
    $(".productList").on('mouseout','.card-box-wrap .card-img img',function(){
    	
   		
    	 if($(window).width()>=1170){
    		   $(this).parent().find('.card-img-info').hide()
    	 }
   	  
	   
	 })
  
	 $('.card-img-info .close').click(function(){
		 $(this).parents('.card-img-info').css('display','none')
	 })
 })
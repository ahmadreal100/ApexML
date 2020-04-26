var Checkout = function () {

    return {
        init: function () {
            
            $('#checkout').on('change', '#checkout-content input[name="account"]', function() {

              var title = '';

              if ($(this).attr('value') == 'register') {
                title = 'مرحله 2 : جزئیات حساب و پرداخت';
              } else {
                title = 'مرحله 2 : اطلاعات پرداخت';
              }    

              $('#payment-address .accordion-toggle').html(title);
            });

        }
    };

}();
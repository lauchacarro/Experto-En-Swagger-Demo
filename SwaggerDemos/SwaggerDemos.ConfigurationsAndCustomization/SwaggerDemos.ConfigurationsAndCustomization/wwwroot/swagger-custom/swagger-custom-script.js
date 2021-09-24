(function () {
    window.addEventListener("load", function () {
        setTimeout(function () {
            var logo = document.getElementsByClassName('link'); //For Changing The Link On The Logo Image
            logo[0].href = "https://www.algeiba.com/";
            logo[0].target = "_blank";
            logo[0].children[0].alt = "Algeiba Swagger";
            logo[0].children[0].src = "https://static.wixstatic.com/media/5efab9_294869c1eb9740e8a74f1d51e883c63e~mv2.png/v1/crop/x_0,y_56,w_1194,h_239/fill/w_252,h_52,al_c,q_85,usm_0.66_1.00_0.01/algeiba-logo_positivo-RGB.webp"; //For Changing The Logo Image
        });
    });
})();

function requestInterceptor(request) {
    request.headers['x-frontend'] = 'swagger';
    return request;
}

function responseInterceptor(response) {
    console.log(response);
    return response;
}
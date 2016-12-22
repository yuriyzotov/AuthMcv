(function ($, win) {

    var _authAes = {};

    _getDecrypted = function (data) {
        q = jQuery.Deferred()

        $.ajax({
            method: 'post',
            url: "/Home/DecryptAES",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({data:data}),
            error: function (data) {
                q.reject("Error decrypting AES data");
            },
            success: function (data) {
                q.resolve({ value: data.value})
            },
        });
        return q;
    }
    // Make a call to the protected Web API by passing in a Bearer Authorization Header
    _getKey = function (onresult) {
        q = jQuery.Deferred()

        $.ajax({
            method: 'get',
            url: "/Home/GetAES256",
            error: function (data) {
                q.reject("Error getting AESKey");
            },
            success: function (data) {
                q.resolve({ key: data.key, iv: data.iv })
            },
        });
        return q;
    }

    _encrypt = function (data, text) {
        //encrypt text
        var keyBytes = CryptoJS.enc.Base64.parse(data.key);
        var ivBytes = CryptoJS.enc.Base64.parse(data.iv);
        var encrypted = CryptoJS.AES.encrypt(text, keyBytes, { iv: ivBytes });
        return encrypted;

    }
    _authAes.getDecrypted = _getDecrypted;
    _authAes.getKey = _getKey;
    _authAes.encrypt = _encrypt;

    win.authAes = _authAes;
}(jQuery, window));

function MakeAjaxRequest(url, callback, method) {
    var xhr;
    if (method == null || method.toLowerCase() == 'post') {
        method = 'GET';
    }

    if (typeof XMLHttpRequest !== 'undefined') xhr = new XMLHttpRequest();
    else {
        var versions = [
            "MSXML2.XmlHttp.5.0",
            "MSXML2.XmlHttp.4.0",
            "MSXML2.XmlHttp.3.0",
            "MSXML2.XmlHttp.2.0",
            "Microsoft.XmlHttp"
        ];

        for (var i = 0, len = versions.length; i < len; i++) {
            try {
                xhr = new ActiveXObject(versions[i]);
                break;
            } catch (e) {
                if(i == len-1) alert("No ActiveXObject support in web worker");
            }
        } 
    }

    xhr.onreadystatechange = ensureReadiness;

    function ensureReadiness() {
        if (xhr.readyState < 4) {
            return;
        }

        if (xhr.status !== 200) {
            return;
        }

        if (xhr.readyState === 4) {
            callback(xhr);
        }
    }

    xhr.open(method, url, true);
    xhr.send('');
}



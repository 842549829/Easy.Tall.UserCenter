const config = {
    baseURL: "http://localhost:51456",
    timeout: 5000
};
function request(o) {
    if (o) {
        o.url = config.baseURL + o.url;
        if (o.data) {
            o.data = JSON.stringify(o.data);
        }
    }
    let index;
    let obj = $.extend(true, {
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "JSON",
        timeout: 50000,
        error: function (e) {
            console.log(e);
        },
        beforeSend: function (xhr) {
            if (xhr) {
                var authorization = localStorage.getItem("Authorization");
                xhr.setRequestHeader("Authorization", authorization);
            }
            //loading层
            index = layer.load(1, {
                shade: [0.1, "#fff"] //0.1透明度的白色背景
            });
        },
        complete: function () {
            layer.close(index);
        }
    }, o);
    $.ajax(obj);
}
function Loader() {
    const self = this;

    self.Get = function (url) {
        return new Promise(function (resolve, reject) {
            if(!url)
                reject();
           
            const xhr = new XMLHttpRequest();
            xhr.open('GET', url, true);
            xhr.onload = function () {
                 if (this.status >= 200 && this.status < 300) {
                     resolve(xhr.response);
                 } else {
                    reject({
                        status: this.status,
                        statusText: xhr.statusText
                    });
                }
            };
            xhr.onerror = function (e) {
                console.error(xhr.statusText);

                reject({
                    status: this.status,
                    statusText: xhr.statusText
                });
            };
            xhr.send();
        });
    }
}
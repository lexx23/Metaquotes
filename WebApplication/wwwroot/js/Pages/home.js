function HomePage() {
    const self = this;
    
    let pageContent;
    self.PageName = 'Home';
    
    const loader = new Loader();

    self.Page = function () {
        return new Promise(function (resolve, reject) {

            if (pageContent) {
                resolve(pageContent);
                return;
            }

            loader.Get(window.location.origin + '/pages/' + self.PageName + ".html").then(function (data) {
                pageContent = data;
                resolve(pageContent);
            }, function() {
                reject();
            });
        });
    }

    self.Bind = function (context) {
    }
}
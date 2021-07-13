function MainApp() {
    const self = this;

    let loader;
    let sideNavigator;
    let pageContent;

    let currentPageNameHeader;
    let currentPageNameText;

    let routes;

    self.initialize = function () {
        sideNavigator = document.getElementById('sideNavigator');
        pageContent = document.getElementById('pageContent');
        loader = document.getElementById('loader');
        currentPageNameHeader = document.getElementById('currentPageNameHeader');
        currentPageNameText = document.getElementById('currentPageNameText');

        routes = {
            '/': new HomePage(),
            '/locations': new LocationsPage(),
            '/geo': new GeoPage()
        }

        for (let route in routes) {
            const htmlRoute = document.createElement('li');
            htmlRoute.id = 'route' + routes[route].PageName;
            htmlRoute.innerHTML = routes[route].PageName;
            htmlRoute.onclick = function () {
                self.onNavigate(route);
            };
            sideNavigator.appendChild(htmlRoute);
        }

        self.onNavigate(window.location.pathname, true);
        setLoaderStatus(false);
    }

    self.onNavigate = function (pathname, force = false) {

        if (!force && pathname === window.location.pathname)
            return;

        if (pathname) {
            if (!routes[pathname])
                pathname = '/';

            setLoaderStatus(true);

            routes[pathname].Page().then(function (data) {
                pageContent.innerHTML = data;
                routes[pathname].Bind();
                setLoaderStatus(false);
            }, function () {
                pageContent.innerHTML = '<h1>Error on page load</h1>';
                setLoaderStatus(false);
            });

            setBreadcrumbText(routes[pathname].PageName);
            setActiveSidebarPage(routes[pathname].PageName);
        }

        if (pathname !== window.location.pathname) {

            window.history.pushState(
                {},
                pathname,
                window.location.origin + pathname
            )
        }
    }

    function setLoaderStatus(isShow) {
        if (!isShow)
            loader.classList.add('d-none');
        else
            loader.classList.remove('d-none');
    }

    function setBreadcrumbText(pageName) {
        currentPageNameHeader.textContent = pageName;

        if (routes['/'].PageName !== pageName)
            currentPageNameText.textContent = pageName;
        else
            currentPageNameText.textContent = '';
    }

    function setActiveSidebarPage(pageName) {
        for (let route in routes) {
            const navElement = document.getElementById('route' + routes[route].PageName);
            if (navElement && routes[route].PageName === pageName)
                navElement.classList.add('active');
            else
                navElement.classList.remove('active');
        }
    }
}
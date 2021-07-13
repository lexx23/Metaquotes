function LocationsPage() {
    const self = this;

    let pageContent;
    self.PageName = 'Locations';

    const loader = new Loader();

    let textInput;
    let searchButton;
    let dataTable;

    self.Page = function () {
        return new Promise(function (resolve, reject) {

            if (pageContent) {
                resolve(pageContent);
                return;
            }

            loader.Get(window.location.origin + '/pages/' + self.PageName + ".html").then(function (data) {
                pageContent = data;
                resolve(pageContent);
            }, function () {
                reject();
            });
        });
    }

    self.Bind = function () {
        textInput = document.getElementById('searchText');
        searchButton = document.getElementById('searchButton');
        dataTable = document.getElementById('dataTable');

        if (textInput)
            textInput.onkeyup = onSearchKeyUp;

        if (searchButton)
            searchButton.onclick = onSearchClick;
    }

    function onSearchKeyUp(e) {
        let searchText = textInput.value;
        if (searchText.length > 0) {
            setSearchButtonStatus(true);
            if(e.keyCode === 13)
                onSearchClick();
        } else
            setSearchButtonStatus(false);
    }

    function onSearchClick() {
        let searchText = textInput.value;

        if (searchText.length > 0 && !searchButton.classList.contains('disabled')) {
            setSearchButtonStatus(false);
            
            loader.Get(window.location.origin + '/city/locations?city=' + searchText).then(function (data) {
                dataTable.innerHTML = '';
                if (data && data.length > 0) {
                    try {
                        const jsonData = JSON.parse(data);
                        for (let i = 0; i < jsonData.length; i++) {
                            dataTable.appendChild(createTableRow(jsonData[i]));
                        }

                    } catch (e) {
                        console.error(e);
                    }
                }

                setSearchButtonStatus(true);
            });
        }
    }

    function setSearchButtonStatus(isShow) {
        if (isShow)
            searchButton.classList.remove('disabled');
        else
            searchButton.classList.add('disabled');
    }

    function createTableRow(row) {
        const tr = document.createElement('tr');
        let data = '';
        for (let column in row) {
            data += '<td>' + row[column] + '</td>';
        }
        tr.innerHTML = data;

        return tr;
    }
}



function GeoPage() {
    const self = this;

    let pageContent;
    self.PageName = 'Geo';

    const loader = new Loader();

    let textInput;
    let searchButton;
    let ipRangeText;
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
        ipRangeText = document.getElementById('ipRangeText');

        if (textInput)
            textInput.onkeyup = onSearchKeyUp;

        if (searchButton)
            searchButton.onclick = onSearchClick;
    }

    function onSearchKeyUp(e) {
        let searchText = textInput.value;
        if (searchText.length > 0 && checkIsIPV4(searchText)) {
            setSearchButtonStatus(true);
            if (e.keyCode === 13)
                onSearchClick();
        } else
            setSearchButtonStatus(false);
    }

    function onSearchClick() {
        let searchText = textInput.value;

        if (searchText.length > 0 && !searchButton.classList.contains('disabled')) {
            setSearchButtonStatus(false);

            loader.Get(window.location.origin + '/ip/location?ip=' + searchText).then(function (data) {
                dataTable.innerHTML = '';
                if (data && data.length > 0) {
                    try {
                        const jsonData = JSON.parse(data);
                        if (jsonData.ipRange)
                            ipRangeText.innerHTML = toStringIp(jsonData.ipRange.ipFrom) + ' - ' + toStringIp(jsonData.ipRange.ipTo);

                        if (jsonData.location)
                            dataTable.appendChild(createTableRow(jsonData.location));

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

    function checkIsIPV4(entry) {
        const blocks = entry.split(".");
        if (blocks.length === 4) {
            return blocks.every(function (block) {
                return parseInt(block, 10) >= 0 && parseInt(block, 10) <= 255;
            });
        }
        return false;
    }

    function toStringIp(ip) {
        return (
            (ip >>> 24) +
            "." +
            ((ip >> 16) & 255) +
            "." +
            ((ip >> 8) & 255) +
            "." +
            (ip & 255)
        )
    }
}

function start() {
    tbody = document.querySelector('tbody');
    tbody.innerHTML = '';
    for (var i = 1; i <= 12; i++) {
        tbody.innerHTML += `<tr id="csoport-${i.toString()}">
                                <td id="fejlec-${i.toString()}" colspan="8" style="background-color: #eee; font-weight: bold; font-size: 1.2rem; text-align: left;">
                                    ${i.toString()}-s csoport
                                </td>
                            </tr>`;
                            // <tr id="csoport-${i.toString()}"></tr>`;
    }
}

function lista(datum) {
    document.querySelector('#datum').innerHTML = datum;
    aktualisDatum = datum;
    letszamok = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
    document.querySelector('tbody').innerHTML = '';
    var myHeaders = new Headers();
    myHeaders.append("Content-Type", "text/html");

    var raw = "{\"datum\":\"2021-11-15\"}";

    var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: '{"datum": "' + datum + '"}',
        redirect: 'follow'
    };

    fetch("https://jedlikinfo.jedlik.eu/api.php/nyitottkapuk-lista", requestOptions)
        .then(response => response.text())
        .then(result => {
            start();
            JSON.parse(result).forEach(element => {
                tr = document.createElement('tr');
                tr.innerHTML = `
                    <td><span class="btn btn-sm btn-danger py-0 cursor-pointer" onClick="torol(${element.id})">Töröl</span></td>
                    <td>${element.fo} fő</td>
                    <td>${element.email}</td>
                    <td>${element.kiseroNeve}</td>
                    <td>${element.tanulo1}</td>
                    <td>${element.tanulo2}</td>
                    <td>${element.tanulo3}</td>
                    <td>${element.tanulo4}</td>`;

                csoportTr = document.querySelector('#csoport-' + element.csoport.toString());
                csoportTr.parentNode.insertBefore(tr, csoportTr.nextSibling);
                
                letszamok[element.csoport - 1] += element.fo;
            });

            for (var i = 1; i <= 12; i++) {
                document.querySelector('#fejlec-' + i.toString()).innerHTML =
                    `${i}-s csoport: összesen: ${letszamok[i - 1]} fő`
            }
        })
        .catch(error => console.log('error', error));
}

function torol(id) {
    deleteId = id;
    document.querySelector('#modalConfirm').style.display = 'block';
    document.querySelector('#modalConfirm').classList.add('show');
}

function closeModal() {
    document.querySelector('#modalConfirm').style.display = 'none';
    document.querySelector('#modalConfirm').classList.remove('show');
}

function torles() {
    var myHeaders = new Headers();
    myHeaders.append("Content-Type", "text/html");

    var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: `{"id": "${deleteId}"}`,
        redirect: 'follow'
    };

    fetch("https://jedlikinfo.jedlik.eu/api.php/nyitottkapuk-torles", requestOptions)
        .then(response => response.text())
        .then(result => {
            closeModal();
            lista(aktualisDatum);
        })
        .catch(error => console.log('error', error));
}
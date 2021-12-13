start();

function start() {
    var myHeaders = new Headers();
    myHeaders.append('Content-Type', 'text/html');
    var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: '{"x":""}',
        redirect: 'follow'
    };

    fetch("https://jedlikinfo.jedlik.eu/api.php/nyitottkapuk-szabadhelyek1", requestOptions)
        .then(response => response.text())
        .then(result => {
            document.querySelector('#regisztracio_1').style.display = 'inline-block';
            if (Number(result) > 1) {
                document.querySelector('#kapacitas_1').innerHTML = result;
            } else {
                document.querySelector('#regisztracio_1').innerHTML = "A szabad helyek megteltek";
                document.querySelector('#regisztracio_1').style.color = '#f00';
            }            
        })
        .catch(error => console.log('error', error));

    fetch("https://jedlikinfo.jedlik.eu/api.php/nyitottkapuk-szabadhelyek2", requestOptions)
        .then(response => response.text())
        .then(result => {
            document.querySelector('#regisztracio_2').style.display = 'inline-block';
            if (Number(result) > 1) {
                document.querySelector('#kapacitas_2').innerHTML = result;
            } else {
                document.querySelector('#regisztracio_2').innerHTML = "A szabad helyek megteltek";
                document.querySelector('#regisztracio_2').style.color = '#f00';
            }
        })
        .catch(error => console.log('error', error));

    fetch("https://jedlikinfo.jedlik.eu/api.php/nyitottkapuk-szabadhelyek3", requestOptions)
        .then(response => response.text())
        .then(result => {
            document.querySelector('#regisztracio_3').style.display = 'inline-block';
            if (Number(result) > 1) {
                document.querySelector('#kapacitas_3').innerHTML = result;
            } else {
                document.querySelector('#regisztracio_3').innerHTML = "A szabad helyek megteltek";
                document.querySelector('#regisztracio_3').style.color = '#f00';
            }
        })
        .catch(error => console.log('error', error));
}

function regisztracio(datum) {
    document.querySelector('#formRegisztracio').style.display = 'block';
    document.querySelector('#divDatumok').style.display = 'none';

    document.querySelector('#lblDatum').innerHTML = '2021 november ' + datum.slice(-2) + '.';
    document.querySelector('#datum').value = datum;
    document.querySelector('#tanulokSzama').value = "1";
    document.querySelector('#divTanuloNeve2').style.display = 'none';
    document.querySelector('#divTanuloNeve3').style.display = 'none';
    document.querySelector('#divTanuloNeve4').style.display = 'none';
    document.querySelector('#divTanulokSzama').style.display = 'none';
    document.querySelector('#KiseroNeve').disabled = true;
    tanuloSzama(1);
}

function tanuloSzama(value) {
    for (let i = 1; i <= Number(value); i++) {
        document.querySelector('#divTanuloNeve' + i.toString()).style.display = 'block';
    }
    for (let i = Number(value) + 1; i <= 4; i++) {
        document.querySelector('#divTanuloNeve' + i.toString()).style.display = 'none';
        document.querySelector('#TanuloNeve' + i.toString()).value = '';
        document.querySelector('#TanuloNeve' + i.toString()).style.backgroundColor = null;
    }
}

function kiseroChecked(value) {
    document.querySelector('#KiseroNeve').disabled = !value;
    if (!value) {
        document.querySelector('#KiseroNeve').value = '';
        document.querySelector('#KiseroNeve').style.backgroundColor = null;
        document.querySelector('#divTanulokSzama').style.display = 'none';
        document.querySelector('#tanulokSzama').value = '1';
        tanuloSzama('1');
    }
    else {
        document.querySelector('#KiseroNeve').focus();
        document.querySelector('#divTanulokSzama').style.display = 'block';
    }
}

function setColor(item) {
    item.style.backgroundColor = null;
    document.querySelector('#error').innerHTML = '';
    document.querySelector('#error').display = 'none';
}

function submit() {
    hiba = false;
    tanulokSzama = Number(document.querySelector('#tanulokSzama').value);
    for (let i = 1; i <= tanulokSzama; i++) {
        if (!document.querySelector('#TanuloNeve' + i.toString()).value) {
            document.querySelector('#TanuloNeve' + i.toString()).style.backgroundColor = 'red';
            document.querySelector('#TanuloNeve' + i.toString()).focus();
            hiba = true;
        }
    }
    if (document.querySelector('#kisero').checked && !document.querySelector('#KiseroNeve').value) {
        document.querySelector('#KiseroNeve').style.backgroundColor = 'red';
        document.querySelector('#KiseroNeve').focus();
        hiba = true;
    }
    if (!document.querySelector('#Email').value) {
        document.querySelector('#Email').style.backgroundColor = 'red';
        document.querySelector('#Email').focus();
        hiba = true;
    }

    if (!hiba) {
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");
        var data = JSON.stringify({
            "datum": document.querySelector('#datum').value,
            "email": document.querySelector('#Email').value,
            "kiserokSzama": document.querySelector('#kisero').checked ? "1" : "0",
            "kiseroNeve": document.querySelector('#KiseroNeve').value,
            "tanulokSzama": tanulokSzama.toString(),
            "tanulo1": document.querySelector('#TanuloNeve1').value,
            "tanulo2": document.querySelector('#TanuloNeve2').value,
            "tanulo3": document.querySelector('#TanuloNeve3').value,
            "tanulo4": document.querySelector('#TanuloNeve4').value
        });

        var requestOptions = {
            method: 'POST',
            headers: myHeaders,
            body: data,
            redirect: 'follow'
        };
        errorMessage = '';
        fetch("https://jedlikinfo.jedlik.eu/api.php/nyitottkapuk-regisztracio", requestOptions)
            .then(response => response.text())
            .then(data => {
                result = JSON.parse(data);
                if (result.id) {
                    document.querySelector('#divSikeresReg').style.display = 'block';
                    document.querySelector('#formRegisztracio').style.display = 'none';
                    document.querySelector('#divSikeresReg h3').innerHTML = `
                        Ön(öke)t a rendszer a ${result.csoport}-s számú csoportba osztotta.<br>
                        A gyülekező a kiválasztott napon 15:00-kor a ${result.terem}-s teremben lesz.`
                    start();
                } else {
                    document.querySelector('#error').style.display = 'block';
                    document.querySelector('#error').innerHTML = result;
                }
            })
            .catch(error => {
                console.log('error', error);
            });
    }

}

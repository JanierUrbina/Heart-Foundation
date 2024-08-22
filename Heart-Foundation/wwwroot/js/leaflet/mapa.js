
            var mymap = L.map('map');
            var mmr = L.marker([0, 0]);
            mymap.setView([12.105, -86.243], 13);
            mmr.bindPopup('0, 0');
            mmr.addTo(mymap);
            L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
                foo: 'bar',
                attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
            }).addTo(mymap);

            mymap.on('click', onMapClick);
            function isll(num) {
                var val = parseFloat(num);
                if (!isNaN(val) && val <= 90 && val >= -90)
                    return true;
                else
                    return false;
            }

            function ConsultarLtLn() {
                var ltdt = $("#latitud").val(); //valor de x en UTM
                var lndt = $("#longitud").val();//valor de y en UTM

                //Ubicar punto en mapa
                UbicarUtmClick(ltdt, lndt, 8);
            }

            //UbicarUtmClick(580031.3567,1337747.1843,10);

            function onMapClick(e) {
                mmr.setLatLng(e.latlng);
                var utcord = e.latlng.utm();
               
                // definir el sistema de proyección UTM
                var utm = "+proj=utm +zone=16 +ellps=WGS84 +datum=WGS84 +units=m +no_defs";
                // convertir a latitud y longitud
                var latLon = proj4(utm).inverse([utcord.x, utcord.y]);
                
                UbicarPuntoUtm(utcord.x, utcord.y, mymap.getZoom()); // Ubica Utm en popup
               
            }


            //ubicar punto en mapa apartir de lat y long desde UTM
            function UbicarUtmClick(lt, ln, zm) {
                UbicarPuntoUtm(lt, ln);

                // definir la proyección UTM
                var utm;
                if (lt > 300000) {
                    utm = "+proj=utm +zone=16 +ellps=WGS84 +datum=WGS84 +units=m +no_defs";
                }
                else
                    if (lt < 300000) {
                        utm = "+proj=utm +zone=17 +ellps=WGS84 +datum=WGS84 +units=m +no_defs";
                    }
                    else {
                        console.log("Zona UTM no soportada");
                        return;
                    }
                // convertir a latitud y longitud
                var latLon = proj4(utm).inverse([parseFloat(lt), parseFloat(ln)]);

                var lat = latLon[1];
                var lon = latLon[0];

                mmr.setLatLng(L.latLng(lat, lon));
                mymap.setView([lat, lon], zm);
            }

            function UbicarPuntoUtm(utmX, utmY) {
                LatitudEnutmX = Number(utmX).toFixed(4);
                LongitudEnutmY = Number(utmY).toFixed(4);
                $("#latitud").val(LatitudEnutmX);
                $("#longitud").val(LongitudEnutmY);

                $("#Ubicacion").val(LatitudEnutmX + ", " + LongitudEnutmY);

                mmr.setPopupContent(LatitudEnutmX + ',' + LongitudEnutmY).openPopup();
            }


            //ubicar punto en mapa apartir de lat y long
            function sm(lt, ln, zm) {
                setui(lt, ln, zm);
                mmr.setLatLng(L.latLng(lt, ln));
                mymap.setView([lt, ln], zm);
            }

            function setui(lt, ln, zm) {
                lt = Number(lt).toFixed(4);
                ln = Number(ln).toFixed(4);
                $("#latitud").val(lt);
                $("#longitud").val(ln);
                mmr.setPopupContent(lt + ',' + ln).openPopup();
            }

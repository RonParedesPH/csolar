    const _ACORN_APP_ELEMENTS_MODIFIED = "acorn_app_elements_modified";    
    const _ACORN_APP_ELEMENTS_HTMLTEXT = "acorn_app_elements_htmltext";
    
    class xtempl {


    constructor() {
        this._init();        
    }


    _init() {
        this._dom = null;
    }


    dom() {
        return this._dom;
    }


    apply(htmlText) {
        const parser = new DOMParser();
        const doc = parser.parseFromString(htmlText, 'text/html');
        this._dom = doc;

        const el = doc.body.querySelectorAll('[data-xtempl-target]')
        el.forEach( e => {
            const t = document.getElementById(e.dataset.xtemplTarget)
            if (t !== null) {
                if (e.dataset.xtemplInsert !== undefined)
                    t.insertBefore(e.cloneNode(true), t.firstChild);
                else 
                    t.appendChild(e.cloneNode(true));
              
                t.setAttribute('data-xtempl-status','done');

            }
        });

    }


    load(url) {
        const callback = (h) => {this.apply(h)};
        if (url) {
            let stub = '_' + url.split('/').join('_').split('.').join('_');

            const xhttp = new XMLHttpRequest();
            xhttp.onreadystatechange = function () {
                if (this.readyState === 4) {
                    if (this.status === 200) {
                        const lastModified = xhttp.getResponseHeader('Last-Modified');
                        console.log('Last Modified:', lastModified);
                        const savetime = localStorage.getItem(_ACORN_APP_ELEMENTS_MODIFIED + stub);
                        const savehtml = localStorage.getItem(_ACORN_APP_ELEMENTS_HTMLTEXT + stub);
                        if (savetime === null ||  
                            savehtml === null || 
                            lastModified !== savetime || 
                            savehtml.length === 0) 
                        {

                            let xhttp1 = new XMLHttpRequest();
                            xhttp1.onreadystatechange = function () {
                                if (this.readyState === 4) {
                                    if (this.status === 200) {
                                        localStorage.setItem(_ACORN_APP_ELEMENTS_MODIFIED + stub, lastModified);
                                        localStorage.setItem(_ACORN_APP_ELEMENTS_HTMLTEXT + stub, this.responseText);
                                        callback(this.responseText);
                                    } 
                                    else {
                                        console.log('xtempl.js error - failed to load ' + url);
                                    }
                                }
                            };
                            xhttp1.open("GET", url, true);
                            xhttp1.send();
                        }
                        else {
                            callback(savehtml);
                        }
                    } 
                    else {
                        console.log('xtempl.js error - failed to load ' + url);
                    }
                }
            };
            xhttp.open("HEAD", url, true);
            xhttp.send();


            return;
        }
    }


    relocate() {
        const el = document.querySelectorAll('[data-xtempl-reloc]')
        el.forEach( e => {
            const t = document.getElementById(e.dataset.xtemplReloc)
            if (t !== null) {
                if (e.dataset.xtemplInsert !== undefined)
                    t.insertBefore(e, t.firstChild);
                else 
                    t.appendChild(e);
              
                t.setAttribute('data-xtempl-status','done');

            }
        });
    }

}

const c = new xtempl();
c.load('/partial/elements.html');

export default xtempl;
/**
 *
 * RowsActive
 *
 * Interface.Plugins.Datatables.RowsAjax page content scripts. Initialized from scripts.js file.
 *
 *
 */


// this.race_racersApi = null;
// this.race_teamsApi = null;
// this.race_classApi = null;

// this.toasts;

function DownloadMaster() {
          mudPool.depends(["/js/api/race_masters.api.js"], (mastersApi) => {
            const api = new mastersApi();
            api.Request_List(
              (data) => {
                const res = JSON.parse(data);
                const link = document.getElementById("hiddenLink");
                link.href = `${api.root()}/${res}`; // Replace with your file URL
                link.download = "Race AppList.xls"; // Replace with your desired file name
                link.click();
              },
              (error) => {
                console.log(error);
              }
            );
          });
        }

        function DownloadRound(roundId) {
          mudPool.depends(["/js/api/race_masters.api.js"], (mastersApi) => {
            const api = new mastersApi();
            api.Request_Round(
              roundId, //<------------- param value
              (data) => {
                const res = JSON.parse(data);
                const link = document.getElementById("hiddenLink");
                link.href = `${api.root()}/${res}`; // Replace with your file URL
                link.download = "Race AppList.xls"; // Replace with your desired file name
                link.click();
              },
              (error) => {
                console.log(error);
              }
            );
          });
        }


class RowsActive {
  constructor() {
    this.racerTab = document.getElementById("datatableRacers").parentElement;
    this.teamTab = document.getElementById("datatableTeams").parentElement;
    this.classTab = document.getElementById("datatableClass").parentElement;
    this.roundTab = document.getElementById("datatableRounds").parentElement;
    this.tabs = [this.racerTab, this.teamTab, this.classTab, this.roundTab];
    this.labels = ["#first", "#second", "#third", "#fourth"];
    this.dataTables = [null, null, null, null];  
  }

  init() {


    this.currentTab = this.racerTab;
    this.currentTab.classList.add("d-block");   
    
    mudPool.depends(
      [
        "/js/api/race_racers.api.js",
        "/js/modules/race.racers.datatable.paged.js",
        "/js/cs/responsivetab.js",
      ],
      (race_racersApi, race_dataTable, responsiveTab) => {
        const r = new race_dataTable(race_racersApi);
        this.dataTables[0] = this.racerTab.querySelector("#datatableRacers");
        r.init(this.dataTables[0]);

        this._onTabClick.bind(this);
        //_switchToTable.bind(this);        
        
        
        const tabs = new responsiveTab(
          document.getElementById("responsiveTabs"),
          {
            onTabClick: (e) => { this._onTabClick(e) },
          }
        );

        

        const elem = document.getElementById("download");
        if (elem != null) {
          //elem.addEventListener("click", DownloadMaster)
          elem.addEventListener("click", (event) => {
            const toast = new Toasts();
            toast.Toast(
              "Are you sure you want to download the masters worksheet",
              "bg-primary",
              true,
              () => {
                DownloadMaster();
              }
            );
          });
        }


      }
    );
  }
  

  _switchToTable(newTab) {
    if (newTab !== null) {
      this.currentTab.classList.remove("d-block");
      this.currentTab.classList.add("d-none");

      newTab.classList.remove("d-none");
      newTab.classList.add("d-block");

      this.currentTab = newTab;
      document
        .querySelectorAll('[data-datatable-shared="true"]')
        .forEach((el) =>
          el.setAttribute(
            "data-datatable",
            "#" +
              this.currentTab
                .querySelector("th")
                .getAttribute("aria-controls")
          )
        );
    }
  };  

  _onTabClick(e) {

    let target = e.getAttribute("data-bs-target");
    switch (target) {
      case "#first":
        this._switchToTable(this.racerTab);
        break;
      case "#second":
        if (this.dataTables[1] == null) {
          mudPool.depends(
            [
              "/js/api/race_teams.api.js",
              "/js/modules/race.teams.datatable.paged.js",
            ],
            (race_teamsApi, race_dataTable) => {
              const r = new race_dataTable(race_teamsApi);
              this.dataTables[1] =
                this.teamTab.querySelector("#datatableTeams");
              r.init(this.dataTables[1]);

              this._switchToTable(this.teamTab);
            }
          );
        } else {
          this._switchToTable(this.teamTab);
        }
        break;

      case "#third":
        if (this.dataTables[2] == null) {
          mudPool.depends(
            [
              "/js/api/race_class.api.js",
              "/js/modules/race.class.datatable.paged.js",
            ],
            (race_classApi, race_classTable) => {
              const r = new race_classTable(race_classApi);
              this.dataTables[2] =
                this.classTab.querySelector("#datatableClass");
              r.init(this.dataTables[2]);

             this._switchToTable(this.classTab);
            }
          );
        } else {
          _switchToTable(this.classTab);
        }
        break;
      case "#fourth":
        if (this.dataTables[3] == null) {
          mudPool.depends(
            [
              "/js/api/race_rounds.api.js",
              "/js/modules/race.rounds.datatable.paged.js",
            ],
            (race_roundsApi, race_datatable) => {
              const r = new race_datatable(race_roundsApi);
              this.dataTables[3] =
                this.roundTab.querySelector("#datatableRounds");
              r.init(this.dataTables[3]);

              this.roundTab.addEventListener("click", (e) => {
                if (e.target.classList.contains("hotlink")) {
                  const toast = new Toasts();
                  toast.Toast(
                    "Are you sure you want to download the round worksheet '" +
                      e.target.dataset.roundName +
                      "'?",
                    "bg-primary",
                    true,
                    () => {
                      DownloadRound(e.target.dataset.roundId);
                    }
                  );
                }
                //  console.log(e)
              });
             this._switchToTable(this.roundTab);
            }
          );
        } else {
         this._switchToTable(this.roundTab);
        }
        break;
    }
  }

}

mudPool.depends([
    "./app.js",
    "./user.js",
    "./user.DomInject.js"
], (app, user, domInject) => {

     mudPool.depends([
        "./base/_helpers.js",
        "./base/_settings.js",
        "./base/_search.js",
        "./base/_nav.js",
        "./_common.js",
        "./_scripts.js",
      ],
      (helpers, settings, search, nav, common, scripts) => {
        const s = new scripts();

        const r = new RowsActive();
        r.init();
      });
});
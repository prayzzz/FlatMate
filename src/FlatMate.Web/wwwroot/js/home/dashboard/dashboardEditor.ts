namespace FlatMate.Home.Dashboard {
    class DashboardEditorModel {
        public types = new Array<DashboardEntryType>();
        public selectedTypeId = "0";
        public values = new Array<DashboardEntryTypeValue>();
        public selectedValueId = "0";
    }

    export class DashboardEditor extends Vue {
        public name = "dashboard-editor";
        public template = "#dashboard-editor-template";

        public $data: DashboardEditorModel;

        public created = this.onCreated;

        public methods = {
            initEditor: this.initEditor
        };

        public watch = {
            'selectedTypeId': this.selectedTypeChanged
        };

        constructor(options?: vuejs.ComponentOption) {
            super(options);
        }

        public onCreated(): void {
            this.$data = this.initEditor();
        }

        private selectedTypeChanged(value: string, old: string): void {
            const self = this;

            const done = (data: DashboardEntryTypeValue[]) => {
                self.$data.values = data;

                if (data.length > 0) {
                    self.$data.selectedValueId = data[0].value;
                }
            }

            const client = new FlatMate.Shared.ApiClient();
            client.get(`home/dashboard/entrytype/${value}`, done);
        }

        private initEditor(): DashboardEditorModel {
            const dataElement = document.getElementById("viewData");
            if (dataElement === null) {
                throw "data missing";
            }

            const data: DashboardEntryType[] = JSON.parse(dataElement.innerHTML);

            const model = new DashboardEditorModel();
            model.types = data;
            model.selectedTypeId = data[0].id;
            model.selectedValueId = "0";

            return model;
        }
    }
}
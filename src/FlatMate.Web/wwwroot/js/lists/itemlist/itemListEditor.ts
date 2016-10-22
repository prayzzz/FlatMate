namespace flatMate.lists.itemLists {
    export class ItemListEditorModel {
        public itemList: ItemList;
    }

    export class ItemListEditor extends Vue {
        public name = 'item-list-editor';
        public template = '#item-list-editor-template';
        public model: ItemListEditorModel;

        public props = {
            'model': {
                default: () => this.initHeroPickerModel()
            }
        }

        public mounted = this.onReady;


        constructor(options?: vuejs.ComponentOption) {
            super(options);
        }

        private onReady(): void {
            console.log("ready")
        }



        private initHeroPickerModel(): flatMate.lists.itemLists.ItemListEditorModel {
            const dataElement = document.getElementById("viewData");
            if (dataElement === null) {
                throw "data missing";
            }

            const data: ItemList = JSON.parse(dataElement.innerHTML);

            const model = new flatMate.lists.itemLists.ItemListEditorModel();
            model.itemList = data;

            return model;
        }
    }
}
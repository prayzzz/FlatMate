namespace FlatMate.Lists.ItemLists {
    class ItemListEditorModel {
        public itemList: ItemList;
        public newGroupName = "";
    }

    export class ItemListEditor extends Vue {
        public name = "item-list-editor";
        public template = "#item-list-editor-template";

        public $data: ItemListEditorModel;

        public methods = {
            saveNewGroup: this.saveNewGroup,
            initEditor: this.initEditor
        }

        public events = {
            "group-deleted": this.deleteGroup
        }

        public created = this.onCreated;

        constructor(options?: vuejs.ComponentOption) {
            super(options);
        }

        public onCreated(): void {
            this.$data = this.initEditor();
        }

        private deleteGroup(group: ItemListGroup): void {
            if (!this.$data.itemList.listGroups) {
                return;
            }

            this.$data.itemList.listGroups.$remove(group);
        }

        private saveNewGroup(): void {
            if (this.$data.newGroupName === "") {
                return;
            }

            // check privileges
            if (!this.$data.itemList.privileges || !this.$data.itemList.privileges.isEditable) {
                return;
            }

            const itemGroup: ItemListGroup = {
                name: this.$data.newGroupName
            };

            const done = (data: ItemListGroup) => {
                this.$data.itemList.listGroups.push(data);
                this.$data.newGroupName = "";
            }

            const client = new FlatMate.Shared.ApiClient();
            client.post(`lists/itemlist/${this.$data.itemList.id}/group/`, itemGroup, done)
        }

        private initEditor(): ItemListEditorModel {
            const dataElement = document.getElementById("viewData");
            if (dataElement === null) {
                throw "data missing";
            }

            const data: ItemList = JSON.parse(dataElement.innerHTML);

            const model = new ItemListEditorModel();
            model.itemList = data;

            return model;
        }
    }
}
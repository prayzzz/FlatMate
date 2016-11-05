namespace FlatMate.Lists.ItemLists {

    class ItemListGroupComponentModel {
        public newItemValue = '';
    }

    export class ItemListGroupComponent extends Vue {
        public group: ItemListGroup;
        public itemlist: ItemList;
        public name = 'item-list-group';
        public props = ['group', 'itemlist'];
        public template = '#item-list-group-template';

        public $data: ItemListGroupComponentModel;

        public methods = {
            saveNewItem: this.saveNewItem,
            deleteGroup: this.deleteGroup,
            showOwner: this.showOwner
        }

        public events = {
            'item-deleted': this.onItemDeleted
        }

        public created = this.onCreated;

        constructor(options?: vuejs.ComponentOption) {
            super(options);
        }

        public onCreated(): void {
            this.$data = new ItemListGroupComponentModel();
        }

        public showOwner(): boolean {
            if ((this.group.privileges && this.group.privileges.isOwned) || (this.group.userId === this.itemlist.userId)) {
                return false;
            }

            return true;
        }

        private onItemDeleted(item: Item): void {
            if (!this.group.items) {
                return;
            }

            this.group.items.$remove(item);
        }

        private deleteGroup(item: Item): void {
            if (!this.group.privileges || !this.group.privileges.isEditable) {
                return;
            }

            const self = this;

            const done = () => {
                this.$dispatch('group-deleted', self.group);
            }

            const client = new FlatMate.Shared.ApiClient();
            client.delete(`lists/itemlist/${this.group.itemListId}/group/${this.group.id}`, done)
        }

        private saveNewItem(): void {
            if (!this.$data.newItemValue || this.$data.newItemValue === "") {
                return;
            }

            if (!this.group.privileges || !this.group.privileges.isEditable) {
                return;
            }

            const item: Item = {
                value: this.$data.newItemValue
            };

            const done = (data: Item) => {
                if (!this.group.items) {
                    this.group.items = new Array<Item>();
                }

                this.group.items.push(data);
                this.$data.newItemValue = '';
            }

            const client = new FlatMate.Shared.ApiClient();
            client.post(`lists/itemlist/${this.group.itemListId}/group/${this.group.id}/item`, item, done)
        }
    }
}
namespace FlatMate.Lists.ItemLists {

    class ItemListGroupComponentModel {
        public newItemValue = '';
    }

    export class ItemListGroupComponent extends Vue {
        public name = 'item-list-group';
        public template = '#item-list-group-template';
        public group: ItemListGroup;

        public props = ['group'];
        public data = () => new ItemListGroupComponentModel();
        public $data: ItemListGroupComponentModel;

        public methods = {
            saveNewItem: this.saveNewItem,
            deleteGroup: this.deleteGroup,
        }

        public events = {
            'item-deleted': this.deleteItem
        }

        constructor(options?: vuejs.ComponentOption) {
            super(options);
        }

        private deleteItem(item: Item): void {
            if (!this.group.items) {
                return;
            }

            this.group.items.$remove(item);
        }

        private deleteGroup(item: Item): void {
            const self = this;

            const done = () => {
                this.$dispatch('group-deleted', self.group);
            }

            const client = new FlatMate.shared.ApiClient();
            client.delete(`lists/itemlist/${this.group.itemListId}/group/${this.group.id}`, done)
        }

        private saveNewItem(): void {
            if (!this.$data.newItemValue || this.$data.newItemValue === "") {
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

            const client = new FlatMate.shared.ApiClient();
            client.post(`lists/itemlist/${this.group.itemListId}/group/${this.group.id}/item`, item, done)
        }
    }
}
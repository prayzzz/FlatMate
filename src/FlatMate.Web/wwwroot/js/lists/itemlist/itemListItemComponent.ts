namespace FlatMate.Lists.ItemLists {

    class ItemListItemComponentModel {
        public isEditMode = false;
    }

    export class ItemListItemComponent extends Vue {
        public name = 'item';
        public template = '#item-list-item-template';
        public item: Item;

        public $data: ItemListItemComponentModel;
        public $els: any;

        public data = () => new ItemListItemComponentModel()
        public props = ['item'];

        public methods = {
            enterEditMode: this.enterEditMode,
            leaveEditMode: this.leaveEditMode,
            saveItem: this.saveItem,
            deleteItem: this.deleteItem
        }

        constructor(options?: vuejs.ComponentOption) {
            super(options);
        }

        private enterEditMode(): void {
            const self = this;

            this.$data.isEditMode = true;

            Vue.nextTick(() => {
                self.$els.iteminput.focus();
            })
        }

        private leaveEditMode(): void {
            this.$data.isEditMode = false;
        }

        private deleteItem(): void {
            const self = this;

            const done = () => {
                this.$dispatch('item-deleted', self.item);
            }

            const client = new FlatMate.shared.ApiClient();
            client.delete(`lists/itemlist/${this.item.itemListId}/group/${this.item.itemListGroupId}/item/${this.item.id}`, done)
        }

        private saveItem(): void {
            const self = this;

            const done = (data: Item) => {
                self.item = data;
                self.leaveEditMode();
            }

            const client = new FlatMate.shared.ApiClient();
            client.put(`lists/itemlist/${this.item.itemListId}/group/${this.item.itemListGroupId}/item/${this.item.id}`, this.item, done)
        }
    }
}
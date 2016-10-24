namespace FlatMate.Lists.ItemLists {

    class ItemListItemComponentModel {
        public isEditMode = false;
        public oldValue = '';
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
            deleteItem: this.deleteItem
        }

        constructor(options?: vuejs.ComponentOption) {
            super(options);
        }

        private enterEditMode(): void {
            const self = this;

            this.$data.isEditMode = true;
            this.$data.oldValue = this.item.value;

            Vue.nextTick(() => {
                self.$els.iteminput.focus();
            })
        }

        private leaveEditMode(): void {
            this.$data.isEditMode = false;

            if (this.$data.oldValue === this.item.value)
            {
                // value not changed
                return;
            }

            // set oldvalue to prevent multiple requests
            this.$data.oldValue = this.item.value;
            
            const client = new FlatMate.shared.ApiClient();
            client.put(`lists/itemlist/${this.item.itemListId}/group/${this.item.itemListGroupId}/item/${this.item.id}`, this.item)
        }

        private deleteItem(): void {
            const self = this;

            const done = () => {
                this.$dispatch('item-deleted', self.item);
            }

            const client = new FlatMate.shared.ApiClient();
            client.delete(`lists/itemlist/${this.item.itemListId}/group/${this.item.itemListGroupId}/item/${this.item.id}`, done)
        }
    }
}
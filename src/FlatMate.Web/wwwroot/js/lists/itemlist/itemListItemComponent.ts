namespace FlatMate.Lists.ItemLists {

    class ItemListItemComponentModel {
        public isEditMode = false;
        public oldValue = '';
    }

    export class ItemListItemComponent extends Vue {
        public item: Item;
        public itemlist: ItemList;
        public name = 'item';
        public props = ['item', 'itemlist'];
        public template = '#item-list-item-template';

        public $data: ItemListItemComponentModel;
        public $els: any;

        public methods = {
            enterEditMode: this.enterEditMode,
            leaveEditMode: this.leaveEditMode,
            deleteItem: this.deleteItem,
            showOwner: this.showOwner,
            moveUp: this.moveUp,
            moveDown: this.moveDown
        }

        public created = this.onCreated;

        constructor(options?: vuejs.ComponentOption) {
            super(options);
        }

        public onCreated(): void {
            this.$data = new ItemListItemComponentModel();
        }

        public showOwner(): boolean {
            if ((this.item.privileges && this.item.privileges.isOwned) || (this.item.userId === this.itemlist.userId)) {
                return false;
            }

            return true;
        }

        private moveUp(): void {
            this.$dispatch('move-item-up', this.item);            
        }

        private moveDown(): void {
            this.$dispatch('move-item-down', this.item);
        }

        private enterEditMode(): void {
            if (!this.item.privileges || !this.item.privileges.isEditable) {
                return;
            }

            const self = this;

            this.$data.isEditMode = true;
            this.$data.oldValue = this.item.value;

            Vue.nextTick(() => {
                self.$els.iteminput.focus();
            })
        }

        private leaveEditMode(): void {
            this.$data.isEditMode = false;

            if (this.$data.oldValue === this.item.value) {
                // value not changed
                return;
            }

            if (this.item.value === "") {
                this.item.value = this.$data.oldValue;
                return;
            }

            // set oldvalue to prevent multiple requests
            this.$data.oldValue = this.item.value;

            const client = new FlatMate.Shared.ApiClient();
            client.put(`lists/itemlist/${this.item.itemListId}/group/${this.item.itemListGroupId}/item/${this.item.id}`, this.item)
        }

        private deleteItem(): void {
            if (!this.item.privileges || !this.item.privileges.isDeletable) {
                return;
            }

            const self = this;

            const done = () => {
                this.$dispatch('item-deleted', self.item);
            }

            const client = new FlatMate.Shared.ApiClient();
            client.delete(`lists/itemlist/${this.item.itemListId}/group/${this.item.itemListGroupId}/item/${this.item.id}`, done)
        }
    }
}
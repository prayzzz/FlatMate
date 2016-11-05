namespace FlatMate.Lists.ItemLists {

    class ItemListItemComponentModel {
        public isEditMode = false;
        public oldValue = '';
        public isEditable = false;
    }

    export class ItemListItemComponent extends Vue {
        public data = () => new ItemListItemComponentModel()
        public item: Item;
        public itemlistOwner: number;
        public name = 'item';
        public props = ['item', 'itemlistOwner'];
        public template = '#item-list-item-template';

        public $data: ItemListItemComponentModel;
        public $els: any;

        public methods = {
            enterEditMode: this.enterEditMode,
            leaveEditMode: this.leaveEditMode,
            deleteItem: this.deleteItem,
            showOwner: this.showOwner
        }

        public created = this.onCreated;

        constructor(options?: vuejs.ComponentOption) {
            super(options);
        }

        public onCreated(): void {
            const currentUser = (new FlatMate.Account.UserService).CurrentUser;

            if (this.itemlistOwner === currentUser.id || this.item.userId === currentUser.id) {
                this.$data.isEditable = true;
            }
        }

        public showOwner(): boolean {
            const currentUser = (new FlatMate.Account.UserService).CurrentUser;

            if (currentUser.id === this.item.userId || this.itemlistOwner === this.item.userId) {
                return false;
            }

            return true;
        }

        private enterEditMode(): void {
            if (!this.$data.isEditable) {
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
            if (!this.$data.isEditable) {
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
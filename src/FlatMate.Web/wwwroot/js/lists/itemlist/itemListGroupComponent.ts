namespace FlatMate.Lists.ItemLists {

    class ItemListGroupComponentModel {
        public newItemValue = '';
        public isEditable = false;
    }

    export class ItemListGroupComponent extends Vue {
        public name = 'item-list-group';
        public template = '#item-list-group-template';
        public group: ItemListGroup;
        public itemlistOwner: number;

        public props = ['group', 'itemlistOwner'];
        public data = () => new ItemListGroupComponentModel();
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
            const currentUser = (new FlatMate.Account.UserService).CurrentUser;

            if (this.itemlistOwner === currentUser.id || this.group.userId === currentUser.id) {
                this.$data.isEditable = true;
            }
        }

        public showOwner(): boolean {
            const currentUser = (new FlatMate.Account.UserService).CurrentUser;

            if (currentUser.id === this.itemlistOwner || currentUser.id === this.group.userId || this.itemlistOwner === this.group.userId) {
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
            if (!this.$data.isEditable) {
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
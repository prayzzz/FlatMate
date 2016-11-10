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
            'item-deleted': this.onItemDeleted,
            'move-item-up': this.onMoveItemUp,
            'move-item-down': this.onMoveItemDown
        }

        public created = this.onCreated;

        constructor(options?: vuejs.ComponentOption) {
            super(options);
        }

        public onCreated(): void {
            this.$data = new ItemListGroupComponentModel();
            this.group.items = this.group.items.sort((a, b) => a.order - b.order);
        }

        private showOwner(): boolean {
            if ((this.group.privileges && this.group.privileges.isOwned) || (this.group.userId === this.itemlist.userId)) {
                return false;
            }

            return true;
        }

        private onMoveItemUp(item?: Item): void {
            if (!item) {
                return;
            }

            const i = this.group.items.indexOf(item)

            if (i === 0) {
                return;
            }

            const previousItem = this.group.items[i - 1];
            const previousOrder = previousItem.order;

            previousItem.order = item.order
            item.order = previousOrder;

            const client = new FlatMate.Shared.ApiClient();
            client.put(`lists/itemlist/${item.itemListId}/group/${item.itemListGroupId}/item/${item.id}`, item);
            client.put(`lists/itemlist/${previousItem.itemListId}/group/${previousItem.itemListGroupId}/item/${previousItem.id}`, previousItem);

            this.group.items = this.group.items.sort((a, b) => a.order - b.order);
        }

        private onMoveItemDown(item?: Item): void {
            if (!item) {
                return;
            }

            const i = this.group.items.indexOf(item)

            if (i === this.group.items.length - 1) {
                return;
            }

            const nextItem = this.group.items[i + 1];
            const nextOrder = nextItem.order;

            nextItem.order = item.order
            item.order = nextOrder;

            const client = new FlatMate.Shared.ApiClient();
            client.put(`lists/itemlist/${item.itemListId}/group/${item.itemListGroupId}/item/${item.id}`, item);
            client.put(`lists/itemlist/${nextItem.itemListId}/group/${nextItem.itemListGroupId}/item/${nextItem.id}`, nextItem);

            this.group.items = this.group.items.sort((a, b) => a.order - b.order);
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
                value: this.$data.newItemValue,
                order: this.group.items.length
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
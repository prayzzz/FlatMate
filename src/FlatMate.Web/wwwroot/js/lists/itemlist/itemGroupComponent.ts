namespace flatMate.lists.itemLists {

    export class ItemListGroupComponent extends Vue {
        public name = 'item-list-group';
        public template = '#item-list-group-template';
        public group: ItemListGroup;
        public props = ['group'];

        constructor(options?: vuejs.ComponentOption) {
            super(options);
        }
    }
}
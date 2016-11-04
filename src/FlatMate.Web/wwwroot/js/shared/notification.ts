namespace FlatMate.Shared {
    export enum NotificationType {
        Error,
        Success
    }

    class NotificationModel {
        private type: NotificationType;
        private message: string;

        constructor(type: NotificationType, message: string) {
            this.type = type;
            this.message = message;
        }

        public get Type(): NotificationType {
            return this.type;
        }

        public get TypeClass(): string {
            return NotificationType[this.type].toLowerCase();
        }

        public get Message(): string {
            return this.message;
        }
    }

    class NotificationBarModel {
        public notifications = new Array<NotificationModel>();

        constructor() {
        }
    }

    export class NotificationService {
        private static instance: NotificationService;
        private model = new NotificationBarModel();

        /**
         * Returns the singleton instance
         */
        constructor() {
            if (!NotificationService.instance) {
                NotificationService.instance = this;
            }

            return NotificationService.instance;
        }

        public Add(type: NotificationType, message: string): void {
            const notification = new NotificationModel(type, message);
            this.model.notifications.push(notification);

            if (type === NotificationType.Success) {
                setTimeout(() => this.model.notifications.$remove(notification), 3000);
            }

            if (type === NotificationType.Error) {
                setTimeout(() => this.model.notifications.$remove(notification), 10000);
            }
        }

        public Dismiss(notification: NotificationModel): void {
            this.model.notifications.$remove(notification);
        }

        public get Model(): NotificationBarModel {
            return this.model;
        }
    }

    export class NotificationBarComponent extends Vue {
        public name = 'notification-bar';
        public template = '#notification-bar-template';

        public data = () => (new NotificationService()).Model;
        public $data: NotificationBarModel;

        constructor(options?: vuejs.ComponentOption) {
            super(options);
        }
    }

    export class NotificationComponent extends Vue {
        public name = 'notification';
        public template = '#notification-template';

        public model: NotificationModel;
        public props = ['model'];

        public methods = {
            dismissNotification: this.dismissNotification
        }

        constructor(options?: vuejs.ComponentOption) {
            super(options);
        }

        private dismissNotification(): void {
            (new NotificationService()).Dismiss(this.model);
        }
    }
}
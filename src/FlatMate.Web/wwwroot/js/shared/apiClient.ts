﻿namespace FlatMate.Shared {
    export interface IApiClient {
        get<TData>(path: string, doneCallback?: (d: TData) => void, failCallback?: (e: IApiError) => void): void;
        put<TData, TResult>(path: string, data: TData, doneCallback?: (d: TResult) => void, failCallback?: (e: IApiError) => void): void;
        post<TData, TResult>(path: string, data: TData, doneCallback?: (d: TResult) => void, failCallback?: (e: IApiError) => void): void;
    }

    export interface IApiError {
        status: number;
        statusText: string;
        responseText: string;
    }

    /**
     * Singleton
     * new ApiV1Client() returns the singleton instance
     */
    export class ApiClient implements IApiClient {
        private static instance: ApiClient;
        private host = `${window.location.protocol}//${window.location.host}/api/v1/`;
        private notificationService = new NotificationService();

        /**
         * Returns the singleton instance
         */
        constructor() {
            if (!ApiClient.instance) {
                ApiClient.instance = this;
            }

            return ApiClient.instance;
        }

        public get<TData>(path: string, doneCallback?: (d: TData) => void, failCallback?: (e: IApiError) => void): void {
            const url = this.host + path;

            atomic.setContentType('application/json');
            atomic.get(url)
                .success((d: TData) => { if (doneCallback) doneCallback(d) })
                .error((e: IApiError) => { if (failCallback) failCallback(e) });
        }

        public delete(path: string, doneCallback?: () => void, failCallback?: (e: IApiError) => void): void {
            const url = this.host + path;

            atomic.setContentType('application/json');
            atomic.delete(url)
                .success(() => { if (doneCallback) doneCallback() })
                .error((e: IApiError, xhr: any) => {
                    let message = e.responseText;
                    if (!message || message === "") {
                        message = xhr.statusText;
                    }

                    this.notificationService.Add(NotificationType.Error, message);
                    if (failCallback) failCallback(e)
                });
        }

        public put<TData, TResult>(path: string, data: TData, doneCallback?: (d: TResult) => void, failCallback?: (e: IApiError) => void): void {
            const url = this.host + path;

            atomic.setContentType('application/json');
            atomic.put(url, JSON.stringify(data))
                .success((d: TResult) => { if (doneCallback) doneCallback(d) })
                .error((e: IApiError) => { if (failCallback) failCallback(e) });
        }

        public post<TData, TResult>(path: string, data: TData, doneCallback?: (d: TResult) => void, failCallback?: (e: IApiError) => void): void {
            const url = this.host + path;

            atomic.setContentType('application/json');
            atomic.post(url, JSON.stringify(data))
                .success((d: TResult) => { if (doneCallback) doneCallback(d) })
                .error((e: IApiError) => { if (failCallback) failCallback(e) });
        }
    }
}

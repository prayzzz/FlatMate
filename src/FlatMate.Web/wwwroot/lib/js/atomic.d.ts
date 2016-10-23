
interface AtomicMethod {
    success<T>(callback: (data: T, xhr: any) => void): AtomicMethod;
    error<T>(callback: (data: T, xhr: any) => void): AtomicMethod;
    always<T>(callback: (data: T, xhr: any) => void): AtomicMethod;
}

interface Atomic {
    setContentType(type: string): void;
    get(url: string): AtomicMethod;
    post(url: string, data?: string): AtomicMethod;
    put(url: string, data?: string): AtomicMethod;
    delete(url: string): AtomicMethod;
}

declare var atomic: Atomic;
export function extractLastKeyPart(key: string): string {
    return key.split(".").pop() || key;
}

export function extractLastKeyParts(keys: string[]): string[] {
    return keys.map(extractLastKeyPart);
}

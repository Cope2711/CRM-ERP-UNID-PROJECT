export const normalizeKeysToLower = (obj: Record<string, any>) =>
    Object.fromEntries(
        Object.entries(obj).map(([k, v]) => [k.toLowerCase(), v])
    );

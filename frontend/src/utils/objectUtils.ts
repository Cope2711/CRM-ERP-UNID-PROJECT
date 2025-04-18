export const normalizeKeysToLower = (obj: Record<string, any>) =>
    Object.fromEntries(
        Object.entries(obj).map(([k, v]) => [k.toLowerCase(), v])
    );

export const extractIsObjectKeyName = (data: Record<string, any>): string => {
    const key = Object.keys(data).find(key =>
        data[key].specialData && data[key].specialData.includes('IsObjectKey')
    );
    
    if (!key) {
        throw new Error('No se encontró ninguna clave con "IsObjectKey" en specialData');
    }
    
    return key;
};
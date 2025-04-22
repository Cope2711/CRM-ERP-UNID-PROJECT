/**
 * Extracts the last part of a key string after the last dot
 */
export const extractLastKeyPart = (key: string): string => {
  const parts = key.split('.');
  return parts[parts.length - 1];
};

/**
 * Extracts the last parts of multiple key strings
 */
export const extractLastKeyParts = (keys: string[]): string[] => {
  return keys.map(key => extractLastKeyPart(key));
}; 
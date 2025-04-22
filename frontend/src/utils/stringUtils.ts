/**
 * Extrae solo la última parte de las claves separadas por punto.
 * Ej: "Branch.BranchId" => "BranchId"
 */
export const extractLastKeyParts = (keys: string[]): string[] =>
  keys.map(key => key.split('.').pop() || key);

export const extractLastKeyPart = (key: string): string =>
  key.split('.').pop() || key;
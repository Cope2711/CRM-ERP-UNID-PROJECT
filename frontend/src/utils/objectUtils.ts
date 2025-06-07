import { RelationSchema } from "@/types/Schema";

export function getChangedValues<T extends Record<string, any>>(
    original: T,
    updated: T
): Partial<T> {
    const result: Partial<T> = {};

    for (const key in updated) {
        const originalValue = original[key];
        const newValue = updated[key];

        const isDateValue = isDate(originalValue) || isDate(newValue);
        const isArray = Array.isArray(originalValue) || Array.isArray(newValue);

        if (isDateValue) {
            const originalTime = originalValue ? new Date(originalValue).getTime() : null;
            const newTime = newValue ? new Date(newValue).getTime() : null;
            if (originalTime !== newTime) {
                result[key] = newValue;
            }
        } else if (isArray) {
            if (JSON.stringify(originalValue) !== JSON.stringify(newValue)) {
                result[key] = newValue;
            }
        } else {
            if (originalValue !== newValue) {
                result[key] = newValue;
            }
        }
    }

    return result;
}

export function extractKeyFieldName(schema: Record<string, any>): string {
    const keyEntry = Object.entries(schema).find(
        ([, value]) => value?.key === true
    );

    return keyEntry ? keyEntry[0] : "";
}

export function schemaToRelationsSchemas(schema: Record<string, any>): RelationSchema[] {
    // Obtener las relaciones de la estructura de esquemas basandonos en el type icollection
    const relationsFields = Object.entries(schema).filter(
        ([, value]) => typeof value?.type === "string" && value.type.toLowerCase().startsWith("icollection")
    );

    // Mapear cada relación a su esquema
    const relationsSchemas: RelationSchema[] = relationsFields.map(([, value]): any => {
        return {
            modelName: value.model,
            controller: value.controller,
            selects: value.selects,
        };
    });

    return relationsSchemas;
}

function isDate(value: any): value is Date {
    return value instanceof Date;
}
export const useRelationData = (schema: Record<string, any>) => {
    return Object.keys(schema).filter(
      (key) =>
        schema[key]?.type?.startsWith("list") &&
        schema[key]?.specialData?.includes("IsRelation")
    );
  };
  
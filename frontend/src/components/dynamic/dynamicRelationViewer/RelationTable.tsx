export const RelationTable = ({
    columns,
    data,
    selectedIds,
    onRowSelect,
  }: {
    columns: string[];
    data: any[];
    selectedIds: string[];
    onRowSelect: (id: string, isSelected: boolean) => void;
  }) => {
    const visibleColumns = columns.length > 1 ? columns.slice(1) : columns;
  
    return (
      <table className="min-w-full divide-y divide-gray-200">
        <thead className="bg-gray-50">
          <tr>
            <th className="px-1 py-1"></th>
            {visibleColumns.map((column) => (
              <th
                key={column}
                className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider"
              >
                {column.split(".").pop()}
              </th>
            ))}
          </tr>
        </thead>
        <tbody className="bg-white divide-y divide-gray-200">
          {data.map((item, index) => {
            const itemId = item[columns[0]];
            const isSelected = selectedIds.includes(itemId);
  
            return (
              <tr
                key={index}
                className={`hover:bg-gray-50 ${isSelected ? "bg-blue-50" : ""}`}
              >
                <td className="px-6 py-4 whitespace-nowrap">
                  <input
                    type="checkbox"
                    checked={isSelected}
                    onChange={(e) => onRowSelect(itemId, e.target.checked)}
                    className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
                  />
                </td>
                {visibleColumns.map((column) => (
                  <td
                    key={`${index}-${column}`}
                    className="px-6 py-4 text-sm text-gray-500"
                  >
                    {item[column]?.toString() || "-"}
                  </td>
                ))}
              </tr>
            );
          })}
        </tbody>
      </table>
    );
  };
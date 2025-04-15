import DynamicUpdateForm from "@/components/dynamicForms/DynamicUpdateForm";

export default function UpdateUserPage({ userId }: { userId: string }) {
    return <DynamicUpdateForm modelName="users" id={userId} />;
}

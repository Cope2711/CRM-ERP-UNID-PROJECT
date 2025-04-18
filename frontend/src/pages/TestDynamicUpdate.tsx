import DynamicUpdateForm from "@/components/dynamic/forms/DynamicUpdateForm";

export default function UpdateUserPage({ userId }: { userId: string }) {
    return <DynamicUpdateForm modelName="users" id={userId} />;
}

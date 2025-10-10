import { FileText } from "lucide-react"
import React from "react"

export function SectionStyled({ title, children }: { title: string; children: React.ReactNode }) {
    return (
        <div className="mb-10">
            <h3 className="text-xl font-semibold text-gray-800 dark:text-white mb-4 flex items-center gap-2">
                <FileText size={20} className="text-primary" /> {title}
            </h3>
            <div>{children}</div>
        </div>
    )
}

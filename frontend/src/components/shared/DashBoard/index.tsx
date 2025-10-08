import React from "react"

const data = [
    { name: "Hired", value: 56 },
    { name: "Pending", value: 24 },
    { name: "Cancelled", value: 18 }
]

const COLORS = ["#00C49F", "#FFBB28", "#FF4D4F"]

export default function DashBoard() {
    return <div className="p-6 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6"></div>
}

// "use client"
// import React from "react"
// import { Tabs, Tab, Card, CardBody } from "@heroui/react"
// import Home from "@/components/shared/Home"

// export default function AccountSidebar() {
//     // const [placement, setPlacement] = React.useState("start")

//     return (
//         <div className="flex flex-col px-4">
//             {/* h1476 */}
//             <div className="flex w-full f lex-col">
//                 <Tabs aria-label="Options" placement="start">
//                     <Tab key="photos" title="Photos">
//                         <Card>
//                             <CardBody>
//                                 <Home />
//                                 Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do
//                                 eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim
//                                 ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut
//                                 aliquip ex ea commodo consequat.
//                             </CardBody>
//                         </Card>
//                     </Tab>
//                     <Tab key="music" title="Music">
//                         <Card>
//                             <CardBody>
//                                 Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris
//                                 nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in
//                                 reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla
//                                 pariatur.
//                             </CardBody>
//                         </Card>
//                     </Tab>
//                     <Tab key="videos" title="Videos">
//                         <Card>
//                             <CardBody>
//                                 Excepteur sint occaecat cupidatat non proident, sunt in culpa qui
//                                 officia deserunt mollit anim id est laborum.
//                             </CardBody>
//                         </Card>
//                     </Tab>
//                 </Tabs>
//             </div>
//         </div>
//     )
// }
// "use client"

// import React from "react"
// import Link from "next/link"
// import { usePathname } from "next/navigation"

// const tabs = [
//     { key: "profile", label: "Tài khoản của tôi", href: "/user/profile" },
//     { key: "orders", label: "Đơn hàng của tôi", href: "/user/my-orders" },
//     { key: "legal", label: "Điều khoản và pháp lý", href: "/user/legal" },
//     { key: "password", label: "Đổi mật khẩu", href: "/user/change-password" }
// ]

// export default function AccountSidebar() {
//     const pathname = usePathname()

//     return (
//         <div className="flex flex-col w-64 bg-gray-50 border-r min-h-screen p-4">
//             {tabs.map((tab) => {
//                 const active = pathname === tab.href
//                 return (
//                     <Link
//                         key={tab.key}
//                         href={tab.href}
//                         className={`block px-4 py-2 rounded-md mb-1 ${
//                             active
//                                 ? "bg-green-100 text-green-600 font-semibold"
//                                 : "text-gray-700 hover:bg-gray-100"
//                         }`}
//                     >
//                         {tab.label}
//                     </Link>
//                 )
//             })}
//         </div>
//     )
// }
"use client"
import React from "react"
import { Tabs, Tab, TabsProps, cn } from "@heroui/react"
import Link from "next/link"
import { usePathname } from "next/navigation"

const tabs = [
    { key: "profile", label: "Tài khoản của tôi", href: "/user/profile" },
    { key: "orders", label: "Đơn hàng của tôi", href: "/user/my-orders" },
    // { key: "legal", label: "Điều khoản và pháp lý", href: "/user/legal" },
    { key: "changePassword", label: "Đổi mật khẩu", href: "/user/change-password" }
]

export default function AccountSidebar(props: TabsProps) {
    const pathname = usePathname()

    return (
        <div className="flex flex-col pr-4">
            <div className="flex w-full flex-col">
                <Tabs
                    color="primary"
                    variant="underlined"
                    aria-label="Options"
                    placement="start"
                    selectedKey={pathname}
                    {...props}
                    className={cn("font-medium text-base", props.className)}
                >
                    {tabs.map((tab) => (
                        <Tab
                            key={tab.href}
                            title={<Link href={tab.href}>{tab.label}</Link>}
                            className={cn(
                                "text-xl gap-6 flex ",
                                tab.key === "changePassword" ? "w-38" : "w-49"
                            )}
                        />
                    ))}
                </Tabs>
            </div>
        </div>
    )
}

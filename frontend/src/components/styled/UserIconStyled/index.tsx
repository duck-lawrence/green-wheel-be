"use client"
import { Dropdown, DropdownTrigger, DropdownMenu, DropdownItem, User } from "@heroui/react"
import Link from "next/link"
import React from "react"

// cách dùng
{
    /* <UserIconStyled
            name="Tony Reichert"
            img="https://i.pravatar.cc/150?u=a042581f4e29026024d"
        /> */
}

//

type UserProps = {
    // props: object
    name: string
    img: string // string
}

export default function UserIconStyled({ name, img }: UserProps) {
    return (
        <div className="flex items-center gap-4">
            <Dropdown placement="bottom-start">
                <DropdownTrigger>
                    <User
                        as="button"
                        avatarProps={{
                            isBordered: true,
                            src: img
                        }}
                        className="transition-transform"
                        name={name}
                        classNames={{
                            name: "text-[16px] font-bold" // chỉnh style cho name
                            // description: "text-sm text-gray-400" // nếu bạn có description
                        }}
                    />
                </DropdownTrigger>
                <DropdownMenu aria-label="User Actions" variant="flat">
                    <DropdownItem key="profile">
                        <Link href="/#">Account information</Link>
                    </DropdownItem>
                    <DropdownItem key="team_settings">
                        <Link href="/#">My orders</Link>
                    </DropdownItem>
                    <DropdownItem key="logout" color="danger">
                        Log out
                    </DropdownItem>
                </DropdownMenu>
            </Dropdown>
        </div>
    )
}

"use client"
import { BreadcrumbItem, Breadcrumbs } from "@heroui/react"
import Link from "next/link"
import React from "react"

export function BreadCrumbsStyled() {
    return (
        <Breadcrumbs>
            {/* <BreadcrumbItem startContent={<HomeIcon />}>Home</BreadcrumbItem> */}

            <BreadcrumbItem>
                <Link href={"/home"}>Home</Link>
            </BreadcrumbItem>
            <BreadcrumbItem>
                <Link href={"/vehical"}>vehicle</Link>
            </BreadcrumbItem>
            <BreadcrumbItem>
                <Link href={"/detail"}>Detail</Link>
            </BreadcrumbItem>
        </Breadcrumbs>
    )
}

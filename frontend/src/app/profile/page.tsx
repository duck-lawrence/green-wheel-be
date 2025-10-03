"use client"
import {
    AvaterStyled,
    ButtonStyled,
    DatePickerStyled,
    InputPhone,
    InputStyled,
    SexPicker
} from "@/components"
import { useProfileStore } from "@/hooks"

import { Input } from "@heroui/react"
import { NotePencilIcon } from "@phosphor-icons/react/dist/ssr"
import React, { useState } from "react"
import { useTranslation } from "react-i18next"
// import { useTranslation } from "react-i18next"
export default function Page() {
    const defaultAvatarUrl = "/images/avtFallback.jpg"
    const { t } = useTranslation()
    const [showChange, setShowChange] = useState(true)
    const { user } = useProfileStore()

    return (
        <div className="w-[984]">
            <div className="text-3xl mb-4 p-4 font-bold">{t("user.account_information")}</div>
            <div className="flex gap-60">
                <div className="ml-6">
                    <AvaterStyled
                        name={`${user?.lastName.trim() || ""} ${user?.firstName.trim() || ""}`}
                        img={user?.avatarUrl || defaultAvatarUrl}
                    />
                </div>

                {showChange ? (
                    <ButtonStyled
                        className="bg-white border border-primary text-primary
                                 hover:bg-primary hover:text-black mt-9"
                        onPress={() => setShowChange(!showChange)}
                    >
                        <div>
                            <NotePencilIcon />
                        </div>
                        {t("user.edit_information")}
                    </ButtonStyled>
                ) : (
                    <ButtonStyled
                        className="bg-white border border-primary text-primary
                                 hover:bg-primary hover:text-black mt-9"
                        onPress={() => setShowChange(!showChange)}
                    >
                        {t("user.save_changes")}
                    </ButtonStyled>
                )}
            </div>
            {showChange === false && (
                <div className="flex justify-center item-center mt-6 ">
                    <InputStyled
                        label={t("user.account_name")}
                        placeholder={t("user.full_name")}
                        variant="bordered"
                        className="w-164"
                    />
                </div>
            )}

            <div className="flex justify-center gap-14 mt-5">
                <DatePickerStyled
                    className="w-75"
                    variant="bordered"
                    {...(showChange === false ? { isDisabled: false } : { isDisabled: true })}
                />
                <SexPicker
                    {...(showChange === false ? { isDisabled: false } : { isDisabled: true })}
                    label={t("user.sex")}
                    color="primary"
                    placeholder={t("user.choose_sex")}
                    variant="bordered"
                    className="w-75"
                />
            </div>
            <div className="flex justify-center gap-14 mt-5 pb-10">
                {/* Phone */}
                <InputPhone style={showChange} />

                {/* Email */}
                <Input
                    {...(showChange === false ? { isDisabled: false } : { isDisabled: true })}
                    className="w-75"
                    color="primary"
                    defaultValue="junior@heroui.com"
                    label="Email"
                    type="email"
                    variant="bordered"
                />
            </div>
        </div>
    )
}

"use client"
import {
    AvaterStyled,
    ButtonStyled,
    DatePickerStyled,
    InputPhone,
    InputStyled,
    SexPicker
} from "@/components"

import { Input } from "@heroui/react"
import { NotePencilIcon } from "@phosphor-icons/react/dist/ssr"
import React, { useState } from "react"
import { useTranslation } from "react-i18next"
// import { useTranslation } from "react-i18next"
export default function Page() {
    const { t } = useTranslation()
    const [showChange, setShowChange] = useState(true)
    const img =
        "https://scontent.fsgn19-1.fna.fbcdn.net/v/t39.30808-6/433446266_758349243066734_884520383743627659_n.jpg?_nc_cat=100&ccb=1-7&_nc_sid=6ee11a&_nc_ohc=FsaTEptwBqwQ7kNvwGBnPfY&_nc_oc=Adl360kkqq9z98joIstrLI_QwmT7Rz78mugSWoMtIDaHnYtsi0LBcoxs5ZVdaHeo9oU&_nc_zt=23&_nc_ht=scontent.fsgn19-1.fna&_nc_gid=uf7J93HxXk7lntMDq36kgQ&oh=00_Afazntj845yvpldFU92bWJNTFamk4xwJTOVFZbYZ2GfZjQ&oe=68DC722B"

    return (
        <div className="w-[984]">
            <div className="text-3xl mb-4 p-4 font-bold">Account information</div>
            <div className="flex gap-60">
                <div className="ml-6">
                    <AvaterStyled name="Ngo Gia Huy" img={img} />
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

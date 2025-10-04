"use client"
import {
    AvatarUploadButton,
    AvatarUploaderModal,
    AvaterStyled,
    ButtonStyled,
    DatePickerStyled,
    DropdownStyle,
    EnumPicker,
    InputStyled
} from "@/components"
import { parseDate } from "@internationalized/date"
import { useAvatarUploadDiscloresureSingleton, useProfileStore, useUpdateMe } from "@/hooks"
import { UserUpdateReq } from "@/models/user/schema/request"
import { NotePencilIcon } from "@phosphor-icons/react/dist/ssr"
import React, { useCallback, useState } from "react"
import { useTranslation } from "react-i18next"
import { useFormik } from "formik"
import * as Yup from "yup"
import { Sex } from "@/constants/enum"
import { SexLabels } from "@/constants/labels"
import dayjs from "dayjs"
import { defaultAvatarUrl } from "@/constants/constants"
import { DropdownItem, DropdownMenu, DropdownTrigger, useDisclosure } from "@heroui/react"

export default function Page() {
    const { t } = useTranslation()
    const user = useProfileStore((s) => s.user)
    const updateUser = useProfileStore((s) => s.updateUser)
    const updateMeMutation = useUpdateMe({ onSuccess: updateUser })
    const [showChange, setShowChange] = useState(true)

    // ===== Upload avatar =====
    const [imgSrc, setImgSrc] = useState<string | null>(null)
    const [croppedAreaPixels, setCroppedAreaPixels] = useState<any>(null)
    const {
        isOpen: isDropdownOpen,
        onOpenChange: onDropdownOpenChange,
        onClose: onDropdownClose
    } = useDisclosure()
    const { onOpen: onAvatarUploadOpen } = useAvatarUploadDiscloresureSingleton()

    const handleSelectFile = (file: File) => {
        const reader = new FileReader()
        reader.addEventListener("load", () => {
            // reset when choose new one
            setImgSrc(reader.result as string)
            onDropdownClose()
            onAvatarUploadOpen()
            // setCrop({ x: 0, y: 0 })
            // setZoom(1)
        })
        reader.readAsDataURL(file)
    }

    // ===== Update Me =====
    const handleUpdateMe = useCallback(
        async (values: UserUpdateReq) => {
            await updateMeMutation.mutateAsync(values)
            setShowChange(!showChange)
        },
        [updateMeMutation, showChange, setShowChange]
    )

    const formik = useFormik({
        enableReinitialize: true,
        initialValues: {
            firstName: user?.firstName || "",
            lastName: user?.lastName || "",
            phone: user?.phone || undefined,
            sex: user?.sex || Sex.Male,
            dateOfBirth: user?.dateOfBirth || ""
        },
        validationSchema: Yup.object({
            firstName: Yup.string()
                .required(t("user.first_name_require"))
                .matches(/^[\p{L}\s]+$/u, t("user.invalid_first_name")),
            lastName: Yup.string()
                .required(t("user.last_name_require"))
                .matches(/^[\p{L}\s]+$/u, t("user.invalid_last_name")),
            phone: Yup.string()
                // .required(t("user.phone_require"))
                .matches(/^(0[0-9]{9})$/, t("user.invalid_phone")),
            sex: Yup.number().required(t("user.sex_require")),
            dateOfBirth: Yup.string().required(t("user.date_of_birth_require"))
        }),
        onSubmit: handleUpdateMe
    })

    return (
        <div>
            {/* Title */}
            <div className="text-3xl mb-4 p-4 font-bold">{t("user.account_information")}</div>

            <AvatarUploaderModal
                imgSrc={imgSrc}
                setImgSrc={setImgSrc}
                croppedAreaPixels={croppedAreaPixels}
                setCroppedAreaPixels={setCroppedAreaPixels}
            />

            {/* Title */}
            <div className="flex justify-between items-center px-36">
                {/* logo and user full name */}
                <div className="flex gap-4 items-center w-fit">
                    <DropdownStyle
                        placement="right-start"
                        classNames={{ content: "min-w-fit max-w-fit" }}
                        isOpen={isDropdownOpen}
                        onOpenChange={onDropdownOpenChange}
                        closeOnSelect={false}
                    >
                        <DropdownTrigger className="w-30 h-30 cursor-pointer">
                            <AvaterStyled src={user?.avatarUrl || defaultAvatarUrl} />
                        </DropdownTrigger>
                        <DropdownMenu variant="flat" classNames={{ base: "p-0 w-fit" }}>
                            <DropdownItem key="upload_avatar" className="block p-0">
                                <AvatarUploadButton onFileSelect={handleSelectFile} />
                            </DropdownItem>
                        </DropdownMenu>
                    </DropdownStyle>

                    <div
                        className="text-3xl" //
                    >{`${user?.lastName.trim() || ""} ${user?.firstName.trim() || ""}`}</div>
                </div>

                {/* Button enable show change */}
                <div>
                    {showChange ? (
                        <ButtonStyled
                            className="border-primary
                                bg-white border text-primary   
                                hover:text-white hover:bg-primary"
                            onPress={() => setShowChange(!showChange)}
                        >
                            <div>
                                <NotePencilIcon />
                            </div>
                            {t("user.edit_information")}
                        </ButtonStyled>
                    ) : (
                        <div className="flex gap-2">
                            <ButtonStyled
                                className="border-primary
                                bg-white border text-primary   
                                hover:text-white hover:bg-primary"
                                isLoading={formik.isSubmitting}
                                isDisabled={!formik.isValid || !formik.dirty}
                                onPress={formik.submitForm}
                            >
                                {t("common.update")}
                            </ButtonStyled>
                            <ButtonStyled
                                isDisabled={formik.isSubmitting}
                                onPress={() => {
                                    setShowChange(!showChange)
                                    formik.resetForm()
                                }}
                            >
                                {t("common.cancel")}
                            </ButtonStyled>
                        </div>
                    )}
                </div>
            </div>

            {/* Form for update */}
            <div className="flex flex-col mt-5 pb-10 gap-2 px-36">
                <div className="flex justify-center gap-2">
                    <InputStyled
                        {...(showChange === false ? { isReadOnly: false } : { isReadOnly: true })}
                        label={t("user.last_name")}
                        variant="bordered"
                        value={formik.values.lastName}
                        onValueChange={(value) => formik.setFieldValue("lastName", value)}
                        isInvalid={
                            !showChange && !!(formik.touched.lastName && formik.errors.lastName)
                        }
                        errorMessage={formik.errors.lastName}
                        onBlur={() => {
                            formik.setFieldTouched("lastName")
                        }}
                    />

                    <InputStyled
                        {...(showChange === false ? { isReadOnly: false } : { isReadOnly: true })}
                        label={t("user.first_name")}
                        variant="bordered"
                        value={formik.values.firstName}
                        onValueChange={(value) => formik.setFieldValue("firstName", value)}
                        isInvalid={
                            !showChange && !!(formik.touched.firstName && formik.errors.firstName)
                        }
                        errorMessage={formik.errors.firstName}
                        onBlur={() => {
                            formik.setFieldTouched("firstName")
                        }}
                    />
                </div>

                <div className="flex justify-center gap-2">
                    {/* Phone */}
                    <InputStyled
                        {...(showChange === false ? { isReadOnly: false } : { isReadOnly: true })}
                        variant="bordered"
                        label={t("user.phone")}
                        maxLength={10}
                        pattern="[0-9]*"
                        onInput={(e) => {
                            e.currentTarget.value = e.currentTarget.value.replace(/[^0-9]/g, "")
                        }}
                        value={formik.values.phone}
                        onValueChange={(value) => formik.setFieldValue("phone", value)}
                        isInvalid={!showChange && !!(formik.touched.phone && formik.errors.phone)}
                        errorMessage={formik.errors.phone}
                        onBlur={() => {
                            formik.setFieldTouched("phone")
                        }}
                    />

                    <EnumPicker
                        {...(showChange === false ? { isReadOnly: false } : { isReadOnly: true })}
                        label={t("user.sex")}
                        labels={SexLabels}
                        value={formik.values.sex}
                        onChange={(val) => formik.setFieldValue("sex", val)}
                    />

                    <DatePickerStyled
                        {...(showChange === false ? { isReadOnly: false } : { isReadOnly: true })}
                        label={t("user.date_of_birth")}
                        isInvalid={
                            !showChange &&
                            !!(formik.touched.dateOfBirth && formik.errors.dateOfBirth)
                        }
                        errorMessage={formik.errors.dateOfBirth}
                        value={
                            formik.values.dateOfBirth
                                ? parseDate(formik.values.dateOfBirth.split("T")[0]) // ✅ convert string → DateValue
                                : null
                        }
                        onChange={(val) => {
                            if (!val) {
                                formik.setFieldValue("dateOfBirth", null)
                                return
                            }

                            const dob = val
                                ? dayjs(val.toDate("Asia/Ho_Chi_Minh")).format("YYYY-MM-DD")
                                : ""

                            formik.setFieldValue("dateOfBirth", dob)
                        }}
                    />
                </div>
            </div>
        </div>
    )
}

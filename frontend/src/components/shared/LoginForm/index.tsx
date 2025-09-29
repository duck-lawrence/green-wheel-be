"use client"
import { Button, Checkbox, Divider, Link } from "@heroui/react"
import React from "react"

// import ButtonGoogle from "../../styled/ButtonGoogle"
import { useFormik } from "formik"
import * as Yup from "yup"
import { ButtonStyled, InputStyled } from "@/components/styled"
import Logo from "@/components/styled/LogoStyled"
import { Icon } from "@iconify/react"
// import { t } from "i18next"
import { useTranslation } from "react-i18next"
export default function Login() {
    const { t } = useTranslation()
    const [isVisible, setIsVisible] = React.useState(false)
    const toggleVisibility = () => setIsVisible(!isVisible)
    const formik = useFormik({
        initialValues: {
            email: "",
            password: "",
            remember: false
        },
        validationSchema: Yup.object().shape({
            email: Yup.string()
                .required(t("email.require"))
                .matches(/^[\w.-]+@[\w.-]+\.[a-zA-Z]{2,}$/, t("email.invalid")),
            password: Yup.string().required(t("password.require")).min(6, t("password.min"))
        }),
        onSubmit: async (values) => {
            await new Promise((resolve) => setTimeout(resolve, 2000))
            alert(JSON.stringify(values))
        }
    })
    return (
        <div className="flex h-full w-full items-center justify-center">
            <div className="rounded-large flex w-full max-w-sm flex-col gap-4">
                <div className="flex flex-col items-center pb-6">
                    {/* <AcmeIcon size={60} /> */}
                    <Logo />
                    <p className="text-xl font-medium">{t("login.welcome")}</p>
                    <p className="text-small text-default-500">{t("login.loginContinue")}</p>
                </div>
                <div className="flex flex-col gap-3">
                    <InputStyled
                        // className="my-3"
                        variant="bordered"
                        label={t("email.label")}
                        value={formik.values.email}
                        onValueChange={(value) => formik.setFieldValue("email", value)}
                        isInvalid={!!(formik.touched.email && formik.errors.email)}
                        errorMessage={formik.errors.email}
                        onBlur={() => {
                            formik.setFieldTouched("email")
                        }}
                        onClear={() => console.log("input cleared")}
                    />
                    <InputStyled
                        variant="bordered"
                        label={t("password.label")}
                        type={isVisible ? "text" : "password"}
                        value={formik.values.password}
                        onValueChange={(value) => formik.setFieldValue("password", value)}
                        isInvalid={!!(formik.touched.password && formik.errors.password)}
                        errorMessage={formik.errors.password}
                        onBlur={() => {
                            formik.setFieldTouched("password")
                        }}
                        endContent={
                            <button
                                aria-label="toggle password visibility"
                                className="focus:outline-solid outline-transparent"
                                type="button"
                                onClick={toggleVisibility}
                            >
                                {isVisible ? (
                                    <Icon
                                        className="text-default-400 pointer-events-none text-2xl"
                                        icon="solar:eye-closed-linear"
                                    />
                                ) : (
                                    <Icon
                                        className="text-default-400 pointer-events-none text-2xl"
                                        icon="solar:eye-bold"
                                    />
                                )}
                            </button>
                        }
                    />
                </div>

                <div className="flex w-full items-center justify-between px-1 py-2">
                    <div className="flex flex-col gap-2">
                        <Checkbox
                            isSelected={formik.values.remember}
                            onValueChange={(isSelected) =>
                                formik.setFieldValue("remember", isSelected)
                            }
                        >
                            {t("login.remember")}
                        </Checkbox>
                    </div>
                    <div>
                        <Link className="text-default-500" href="/forgot" size="sm">
                            {t("login.forgot")}
                        </Link>
                    </div>
                </div>

                <ButtonStyled
                    className="w-full"
                    type="submit"
                    isLoading={formik.isSubmitting}
                    color="primary"
                    isDisabled={!formik.isValid}
                    onPress={() => formik.submitForm()}
                >
                    {t("login.login")}
                </ButtonStyled>

                <div className="flex items-center gap-4 py-2">
                    <Divider className="flex-1" />
                    <p className="text-tiny text-default-500 shrink-0">{t("login.or")}</p>
                    <Divider className="flex-1" />
                </div>
                <div className="flex flex-col gap-2">
                    <Button
                        startContent={<Icon icon="flat-color-icons:google" width={24} />}
                        variant="bordered"
                        onPress={() => (window.location.href = "/api/auth/google")} // Redirect ra Google OAuth
                    >
                        {t("login.continueWithGoogle")}
                    </Button>
                </div>
                <p className="text-small text-center">
                    {t("login.needToCreateAnAccount")}&nbsp;
                    <Link href="/signup" size="sm">
                        {t("login.regis")}
                    </Link>
                </p>
            </div>
        </div>
    )
}

'use client'

import React, { useState, useEffect } from 'react'
import { Formik, Form, Field, ErrorMessage } from 'formik'
import * as Yup from 'yup'
import { Icon } from '@iconify/react'
import { useTranslation } from 'react-i18next'
import Link from 'next/link'
import { useProfileStore, useToken } from '@/hooks'

// Validation schema
const RegisterCarSchema = Yup.object().shape({
  fullName: Yup.string().required('Tên người thuê là bắt buộc'),
  phone: Yup.string()
    .matches(/^[0-9]{9,11}$/, 'Số điện thoại không hợp lệ')
    .required('Số điện thoại là bắt buộc'),
  email: Yup.string().email('Email không hợp lệ').required('Email là bắt buộc'),
  isVingroup: Yup.boolean(),
  pickupLocation: Yup.string().required('Địa chỉ nhận xe là bắt buộc'),
  referralCode: Yup.string(),
  note: Yup.string(),
  paymentMethod: Yup.string().required('Phương thức thanh toán là bắt buộc'),
  promotionCode: Yup.string(),
  agreeTerms: Yup.boolean().oneOf([true], 'Bạn phải đồng ý với điều khoản thanh toán'),
  agreeDataPolicy: Yup.boolean().oneOf([true], 'Bạn phải đồng ý với chính sách dữ liệu')
})

const RegisterReceiveCar = () => {
  const { t } = useTranslation()
  const [mounted, setMounted] = useState(false)
  const [useVpoints, setUseVpoints] = useState(false)
  const [includeInsurance, setIncludeInsurance] = useState(false)
  const { user } = useProfileStore()
  const isLoggedIn = useToken((s) => !!s.accessToken)

  // Đảm bảo component chỉ render ở client để tránh lỗi hydration
  useEffect(() => {
    setMounted(true)
  }, [])

  const initialValues = {
    fullName: isLoggedIn && user ? `${user.firstName} ${user.lastName}` : '',
    phone: isLoggedIn && user && user.phone ? user.phone : '',
    email: isLoggedIn && user ? user.email : '',
    isVingroup: false,
    pickupLocation: '',
    referralCode: '',
    note: '',
    paymentMethod: '',
    promotionCode: '',
    agreeTerms: false,
    agreeDataPolicy: false
  }

  const handleSubmit = (values) => {
    console.log('Form values:', values)
    // Xử lý gửi form
  }

  // Không render gì nếu component chưa được mount ở client
  if (!mounted) {
    return null
  }

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-lg max-w-5xl w-full max-h-[90vh] overflow-y-auto">
        <div className="flex justify-between items-center p-6 ">  {/* Thêm Border-b nếu muốn có dòng kẻ */}
          <h2 className="text-3xl font-bold">ĐĂNG KÝ THUÊ XE</h2>
          <button className="text-gray-500 hover:text-gray-700">
            <Icon icon="ph:x" width={24} height={24} />
          </button>
        </div>

        <Formik
          initialValues={initialValues}
          validationSchema={RegisterCarSchema}
          onSubmit={handleSubmit}
        >
          {({ errors, touched, values, setFieldValue }) => (
            <Form className="p-6">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div>
                  {/* Thông tin người thuê */}
                  <div className="space-y-4">
                    <div>
                      <label htmlFor="fullName" className="block text-sm font-medium mb-1">
                        Tên người thuê<span className="text-red-500">*</span>
                      </label>
                      <Field
                        type="text"
                        id="fullName"
                        name="fullName"
                        placeholder="Nguyễn Văn A"
                        className={`w-full border rounded-md p-2 ${errors.fullName && touched.fullName ? 'border-red-500' : 'border-gray-300'}`}
                      />
                      <ErrorMessage name="fullName" component="div" className="text-red-500 text-sm mt-1" />
                    </div>

                    <div>
                      <label htmlFor="phone" className="block text-sm font-medium mb-1">
                        Số điện thoại<span className="text-red-500">*</span>
                      </label>
                      <div className="relative">
                        <Field
                          type="text"
                          id="phone"
                          name="phone"
                          placeholder="09xxxxx"
                          className={`w-full border rounded-md p-2 ${errors.phone && touched.phone ? 'border-red-500' : 'border-gray-300'}`}
                        />
             
                      </div>
                      <ErrorMessage name="phone" component="div" className="text-red-500 text-sm mt-1" />
                      <p className="text-xs text-red-500 mt-1">SĐT có ít nhất 9 kí tự</p>
                      <p className="text-xs text-red-500">Vui lòng xác thực số điện thoại để sử dụng các dịch vụ của Green Wheel</p>
                    </div>

                    <div>
                      <label htmlFor="email" className="block text-sm font-medium mb-1">
                        Email<span className="text-red-500">*</span>
                      </label>
                      <Field
                        type="email"
                        id="email"
                        name="email"
                        placeholder="wheel@fpt.edu.vn"
                        className={`w-full border rounded-md p-2 ${errors.email && touched.email ? 'border-red-500' : 'border-gray-300'}`}
                      />
                      <ErrorMessage name="email" component="div" className="text-red-500 text-sm mt-1" />
                    </div>


                    <div>
                      <label htmlFor="pickupLocation" className="block text-sm font-medium mb-1">
                        Nơi nhận xe<span className="text-red-500">*</span>
                      </label>
                      <div className="relative">
                        <Field
                          as="select"
                          id="pickupLocation"
                          name="pickupLocation"
                          className={`w-full border rounded-md p-2 appearance-none ${errors.pickupLocation && touched.pickupLocation ? 'border-red-500' : 'border-gray-300'}`}
                        >
                          <option value="">Chọn địa chỉ chi tiết</option>
                          <option value="place1">Place 1</option>
                          <option value="place2">Place 2</option>
                        </Field>
                        <div className="absolute right-2 top-1/2 -translate-y-1/2 pointer-events-none">
                          <Icon icon="ph:caret-down" />
                        </div>
                      </div>
                      <ErrorMessage name="pickupLocation" component="div" className="text-red-500 text-sm mt-1" />
                    </div>

                  

                    <div>
                      <label htmlFor="note" className="block text-sm font-medium mb-1">
                        Ghi chú
                      </label>
                      <Field
                        as="textarea"
                        id="note"
                        name="note"
                        className="w-full border rounded-md p-2 h-24 border-gray-300"
                      />
                    </div>
                  </div>

                  {/* Phương thức thanh toán */}
                  <div className="mt-6">
                    <h3 className="font-medium mb-3">Phương thức thanh toán</h3>
                    <Field
                      as="select"
                      id="paymentMethod"
                      name="paymentMethod"
                      className={`w-full border rounded-md p-2 appearance-none ${errors.paymentMethod && touched.paymentMethod ? 'border-red-500' : 'border-gray-300'}`}
                    >
                      <option value="">Chọn phương thức thanh toán</option>
                      <option value="banking">Chuyển khoản ngân hàng</option>
                      <option value="momo">Ví MoMo</option>
                    </Field>
                    <ErrorMessage name="paymentMethod" component="div" className="text-red-500 text-sm mt-1" />
                  </div>

                

                  {/* Điều khoản */}
                  <div className="mt-6 space-y-3">
                    <div className="flex items-start">
                      <Field
                        type="checkbox"
                        id="agreeTerms"
                        name="agreeTerms"
                        className="mt-1"
                      />
                      <label htmlFor="agreeTerms" className="ml-2 text-sm">
                        Đã đọc và đồng ý với <Link href="#" className="text-blue-600 hover:underline">Điều khoản thanh toán</Link> của Green Wheel
                      </label>
                    </div>
                    {errors.agreeTerms && touched.agreeTerms && (
                      <div className="text-red-500 text-sm">{errors.agreeTerms}</div>
                    )}

                    <div className="flex items-start">
                      <Field
                        type="checkbox"
                        id="agreeDataPolicy"
                        name="agreeDataPolicy"
                        className="mt-1"
                      />
                      <label htmlFor="agreeDataPolicy" className="ml-2 text-sm">
                        Tôi đồng ý để lại thông tin cá nhân theo <Link href="#" className="text-blue-600 hover:underline">Điều khoản chia sẻ dữ liệu cá nhân</Link> của Green Wheel
                      </label>
                    </div>
                    {errors.agreeDataPolicy && touched.agreeDataPolicy && (
                      <div className="text-red-500 text-sm">{errors.agreeDataPolicy}</div>
                    )}
                  </div>
                </div>

                <div>
                  {/* Thông tin xe */}
                  <div className="bg-gray-50 p-4 rounded-lg">
                    <div className="flex items-center space-x-4">
                      <div className="w-32 h-24 relative">
                        <img 
                          src="/cars/vinfast-vf3.jpg" 
                          alt="VinFast VF 3"
                          className="rounded-md w-full h-full object-cover"
                          onError={(e) => {
                            e.currentTarget.src = "/images/avtFallback.jpg";
                          }}
                        />
                      </div>
                      <div>
                        <h3 className="text-lg font-medium">VinFast VF 3</h3>
                      </div>
                    </div>

                    <div className="mt-4 bg-blue-50 p-4 rounded-lg">
                      <div className="flex justify-between items-center">
                        <h4 className="font-medium">Hà Nội</h4>
                        <button className="text-blue-600">
                          <Icon icon="ph:pencil" className="inline mr-1" />
                        </button>
                      </div>
                      <div className="mt-2 flex items-center">
                        <span>1 ngày • 16:50 24/09/2025 → 16:50 25/09/2025</span>
                      </div>
                      <div className="mt-1">
                        <span className="text-sm">Hình thức thuê: Theo ngày</span>
                      </div>
                    </div>

                    <div className="mt-4">
                      <h4 className="font-medium">Bảng kê chi tiết</h4>
                      <div className="mt-2 space-y-2">
                        <div className="flex justify-between">
                          <span>Cước phí niêm yết</span>
                          <span>590.000đ</span>
                        </div>
                        <div className="border-t pt-2 flex justify-between font-medium">
                          <span>Tổng tiền</span>
                          <span>590.000đ</span>
                        </div>
                        <div className="flex justify-between">
                          <span>Tiền đặt cọc</span>
                          <span>5.000.000đ</span>
                        </div>
                      </div>
                    </div>


                    <div className="mt-6 border-t pt-4">
                      <div className="flex justify-between items-center">
                        <div>
                          <span className="font-medium">Thanh toán</span>
                          <span className="text-xs text-gray-500 block">*Giá thuê xe đã bao gồm VAT.</span>
                        </div>
                        <span className="text-2xl font-bold text-green-500">5.590.000đ</span>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <div className="mt-8 text-center">
                <button 
                  type="submit" 
                  className="bg-green-600 hover:bg-green-700 text-white px-8 py-2 rounded-md"
                >
                  Thanh toán 5.590.000đ
                </button>
              </div>
            </Form>
          )}
        </Formik>
      </div>
    </div>
  )
}

export default RegisterReceiveCar
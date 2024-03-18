import { useEffect, useState } from "react"
import { Form, message } from "antd"
import alovaInstance, { IReturnResult } from "@/utils/request"
import { urls } from "@/api/urls"
import { useTime } from "@/utils";
export interface IDetailModal {
    id: string;
    inPrice: number;
    outPrice: number;
    outAll: number;
    productCode: string;
    productCount: number;
    productModel: string;
    productName: string;
    returnCount: number;
}
const useOutForm = () => {
    const [title, setTitle] = useState<string>('创建退货单')
    const [visiable, setVisiable] = useState<boolean>(false)
    const [puraseForm] = Form.useForm()
    const [loadId, setLoadId] = useState<string>('')
    const [detailModel, setDetailModel] = useState<Array<IDetailModal>>()
    const [totalDetail, setTotalDetail] = useState<number>(0)
    const [isCreate, setIsCreate]  = useState<boolean>(false)
    const onMoalSave = (data: any) => {
        setVisiable(false)
        puraseForm.setFieldValue('inOrderCode', data.code)
        puraseForm.setFieldValue('inUser', data.userName)
        puraseForm.setFieldValue('inPhone', data.userTel)
        puraseForm.setFieldValue('supilerId', data.supilerId)
        setLoadId(data.id)
    }
    const onModalClose = () => {
        setVisiable(false)
    }
    const onSelectClick = () => {
        setVisiable(true)
    }
    useEffect(() => {
        async function init() {
            await loadDetail()
        }
        init()
    }, [loadId])
    const handlerRemove = (record: IDetailModal) => {
        const data = detailModel.filter(x => x.productCode !== record.productCode)
        setDetailModel(data)
    }
    const onRowEdit = (code: string, props: string, value: any) => {
        const newTable = [...detailModel]
        const findex = newTable.findIndex(x => x.productCode === code)
        if (findex > -1) {
            newTable[findex][props] = value
        }
        setDetailModel(newTable)
    }
    const loadDetail = async () => {
        if (loadId) {
            debugger
            const { success, data } = await alovaInstance.Get<IReturnResult>(urls.getPurashModalDetail + "/" + loadId).send();
            if (success) {
                setDetailModel(data)
                setTotalDetail(data.length)
            }
        }
    }
    const onSave = async () => {
        const time = puraseForm.getFieldValue('outOrderTime')
        if (!time) {
            message.error('请选择退货时间')
            return
        }
        const supilerId = puraseForm.getFieldValue('supilerId')
        if (!supilerId) {
            message.error('无效的进货单据数据')
            return
        }
        const code = puraseForm.getFieldValue('inOrderCode')
        if (!code) {
            message.error('请输入选择进货单据')
            return
        }
        const inUser = puraseForm.getFieldValue('inUser')
        if (!inUser) {
            message.error('请输入收货方联系人')
            return
        }
        const inPhone = puraseForm.getFieldValue('inPhone')
        if (!inPhone) {
            message.error('请输入收货方联系电话')
            return
        }
        const remark = puraseForm.getFieldValue('remark')
        if (!remark) {
            message.error('请输入退货原因')
            return
        }
        if (detailModel.length <= 0) {
            message.error('此进货单据异常，无退货明细，无法提交')
            return
        }
        const postData = {
            outOrderTime: useTime(time),
            inOrderCode: code,
            inUser: inUser,
            inPhone: inPhone,
            reason: remark,
            detail: detailModel,
            supilerId: supilerId,
            logics: puraseForm.getFieldValue('logistics') ?? ''
        }
        const { success, message: msg } =
            await alovaInstance.Post<IReturnResult, any>(urls.createOutPurashe, postData)
                .send()
        setIsCreate(success)
        if (success) {
            message.success(msg)
        } else {
            message.error(msg)
        }
        
    }
    return {
        title,
        visiable,
        puraseForm,
        onMoalSave,
        onModalClose,
        onSelectClick,
        detailModel,
        handlerRemove,
        onRowEdit,
        onSave,
        isCreate
    }
}
export {
    useOutForm
}
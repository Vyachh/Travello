export interface TripForm{
    userId: string
    title: string
    description: string
    dateFrom: string
    dateTo: string
    price: number
    author: string
    image: File | null
}

// formData.append('userId', this.accountService.userInfo.id);
//       formData.append('title', this.tripForm.value.title || '');
//       formData.append('description', this.tripForm.value.description || '');
//       formData.append('dateFrom', dateStart?.toString() || '');
//       formData.append('dateTo', dateEnd?.toString() || '');
//       formData.append('price', this.tripForm.value.price || '');
//       formData.append('author', this.accountService.userInfo.userName);
//       formData.append('image', this.tripImage);